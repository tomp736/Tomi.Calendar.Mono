using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomi.Calendar.Mono.Shared
{
    public static class DataTableHelper
    {
        public static DataTable GenerateTable(string tableName, int columns, int rows)
        {
            DataTable dt = new DataTable(tableName);
            for(int i = 0; i < columns; i++)
            {
                dt.Columns.Add(new DataColumn($"column {i}"));
            }

            DataRow row;
            for (int i = 0; i < rows; i++)
            {
                row = dt.NewRow();
                row.ItemArray = Enumerable.Range(0,columns).Select(n => $"text for row {i} column {n}").ToArray();
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static void SerializeDataTable(BinaryTypesWriter bw, DataTable table)
        {
            Type[] columnTypes = new Type[table.Columns.Count];

            //write table name
            bw.Write(table.TableName);

            //write columns count
            bw.Write7BitInt(table.Columns.Count);

            for (int i = 0; i < columnTypes.Length; i++)
            {
                //write column name and type
                bw.Write(table.Columns[i].ColumnName);
                bw.Write(table.Columns[i].DataType.FullName);

                columnTypes[i] = table.Columns[i].DataType;
            }

            //write rows count
            bw.Write7BitInt(table.Rows.Count);

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < columnTypes.Length; i++)
                {
                    if (row.IsNull(i))
                    {
                        bw.Write("");
                        continue;
                    }

                    if (columnTypes[i] == typeof(System.String))
                        bw.Write((string)row[i]);
                    else if (columnTypes[i] == typeof(System.Int32))
                        bw.Write7BitInt((int)row[i]);
                    else if (columnTypes[i] == typeof(System.Int64))
                        bw.Write7BitLong((long)row[i]);
                    else if (columnTypes[i] == typeof(System.Decimal))
                        bw.WriteCompactDecimal((decimal)row[i]);
                    else if (columnTypes[i] == typeof(System.DateTime))
                        bw.WriteCompactDateTime((DateTime)row[i], TimeSpan.TicksPerMillisecond * 100);
                    else if (columnTypes[i] == typeof(bool))
                        bw.Write((bool)row[i]);
                }
            }
        }

        public static DataTable DeserializeDataTable(BinaryTypesReader br)
        {
            DataTable table = new DataTable();
            table.TableName = br.ReadString();

            int columnCount = br.Read7BitInt();

            Type[] columnTypes = new Type[columnCount];

            for (int i = 0; i < columnCount; i++)
            {
                string columnName = br.ReadString();
                string typeName = br.ReadString();
                Type columnType = Type.GetType(typeName);

                DataColumn col = new DataColumn(columnName, columnType);
                table.Columns.Add(col);

                columnTypes[i] = columnType;
            }

            int rowsCount = br.Read7BitInt();
            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                DataRow row = table.NewRow();
                table.Rows.Add(row);

                for (int i = 0; i < columnCount; i++)
                {
                    if (columnTypes[i] == typeof(System.String))
                        row[i] = br.ReadString();
                    else if (columnTypes[i] == typeof(System.Int32))
                        row[i] = br.Read7BitInt();
                    else if (columnTypes[i] == typeof(System.Int64))
                        row[i] = br.Read7BitLong();
                    else if (columnTypes[i] == typeof(System.Decimal))
                        row[i] = br.ReadCompactDecimal();
                    else if (columnTypes[i] == typeof(System.DateTime))
                        row[i] = br.ReadCompactDateTime(TimeSpan.TicksPerMillisecond * 100);
                    else if (columnTypes[i] == typeof(bool))
                        row[i] = br.ReadBoolean();
                }
            }

            return table;
        }

        public class BinaryTypesReader : BinaryReader
        {
            public BinaryTypesReader(Stream input)
                : base(input)
            {

            }

            public BinaryTypesReader(Stream input, Encoding encoding)
                : base(input, encoding)
            {

            }

            public int Read7BitInt()
            {
                return (int)Read7BitLong();
            }


            public decimal ReadCompactDecimal()
            {
                int exponent = Read7BitInt();
                long mantisa = Read7BitLong();
                return mantisa / (decimal)Math.Pow(10, exponent);
            }

            //Note: if we know the digits in advance - we save the reading/writing of the exponent
            public decimal ReadCompactDecimal(int digits)
            {
                long mantisa = Read7BitLong();
                return mantisa / (decimal)Math.Pow(10, digits);
            }

            public long Read7BitLong()
            {
                long result = 0;
                bool isNeg = false;
                long curByte;
                int loopCount = 0;
                bool done = false;
                int curShift = 0;
                int nextShiftDiff = 6;
                byte mask = 0x3F; //first mask: msb for 'has more' msb-1 for sign
                bool isMinValue = false; //Note: long.MinValue has no equivalent positive value
                do
                {
                    curByte = ReadByte();
                    if (loopCount == 0)
                        isNeg = (curByte & 0x40) > 0;
                    if ((curByte & 0x80) == 0)
                        done = true;
                    if (loopCount == 9 && (curByte & 0x40) > 0)
                        isMinValue = true;
                    curByte &= mask;
                    mask = 0x7F;
                    result |= (curByte << curShift);
                    curShift += nextShiftDiff;
                    nextShiftDiff = 7;
                    ++loopCount;
                }
                while (!done);

                if (isMinValue)
                    return long.MinValue;
                return result * (isNeg ? -1 : 1);
            }

            public ulong Read7BitULong()
            {
                ulong result = 0;
                ulong curByte;
                bool done = false;
                int curShift = 0;
                int nextShiftDiff = 7;
                byte mask = 0x7F;
                do
                {
                    curByte = ReadByte();
                    if ((curByte & 0x80) == 0)
                        done = true;
                    curByte &= mask;
                    result |= (curByte << curShift);
                    curShift += nextShiftDiff;
                }
                while (!done);

                return result;
            }

            public DateTime ReadDateTime()
            {
                return new DateTime(ReadInt64());
            }

            //ticksResolution: e.g. TimeSpan.TicksPerSecond or TimeSpan.TicksPerhour
            public DateTime ReadCompactDateTime(long ticksResolution)
            {
                return new DateTime(Read7BitLong() * ticksResolution);
            }

            public TimeSpan ReadTimeSpan()
            {
                return new TimeSpan(Read7BitLong());
            }

            public byte[] ReadBytesArray()
            {
                int count = Read7BitInt();
                return (count > 0 ? ReadBytes(count) : null);
            }
        }

        public class BinaryTypesWriter : BinaryWriter
        {
            public BinaryTypesWriter(Stream output)
                : base(output)
            {

            }

            public BinaryTypesWriter(Stream output, Encoding encoding)
                : base(output, encoding)
            {

            }

            public void Write7BitInt(int value)
            {
                Write7BitLong(value);
            }

            public void WriteCompactDecimal(decimal val)
            {
                //val = Decimal.Round(val, 5);
                string asString = val.ToString();
                // TODO: -=-= Check this on a French localized computer
                string[] split = asString.Split(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);

                int exponent = 0;
                if (split.Length > 1)
                {
                    exponent = split[1].Length;
                }
                else
                {
                    int index = split[0].Length - 1;
                    while (index > 0)
                    {
                        if (split[0][index] == '0')
                        {
                            exponent--;
                        }
                        else
                        {
                            break;
                        }
                        index--;
                    }
                }
                if (exponent > 6) exponent = 6;//no more than 6 digits
                long mantisa = (long)((val * (decimal)Math.Pow(10, exponent))); //InstrumentPrice.ConvertToIntegerPrice(, );
                Write7BitInt(exponent);
                Write7BitLong(mantisa);
            }

            //Note: if we know the digits in advance - we save the reading/writing of the exponent
            public void WriteCompactDecimal(decimal value, int digits)
            {
                int sign = value > 0 ? 1 : -1;
                long mantisa = (long)((value * (decimal)Math.Pow(10, digits)) + (sign * 0.5M));
                Write7BitLong(mantisa);
            }




            public new void Write(string value)
            {
                base.Write(value == null ? "" : value);
            }

            public void Write7BitLong(long value)
            {
                bool isNeg = value < 0;
                bool isMinValue = false;
                if (value == long.MinValue)
                {
                    isMinValue = true;
                    value += 1; //Note: long.MinValue has no equivalent positive value
                }
                value = Math.Abs(value);
                byte lowestByte;
                byte mask = 0x3F; //first mask: msb for 'has more' msb-1 for sign
                int shift = 6;
                int loopCount = 0;
                do
                {
                    lowestByte = (byte)(value & mask);
                    value >>= shift;
                    if (value != 0)
                        lowestByte |= 0x80;
                    if (shift == 6 && isNeg) //shift == 6 only in first byte
                        lowestByte |= 0x40;
                    if (loopCount == 9 && isMinValue)
                        lowestByte |= 0x40;
                    mask = 0x7F;
                    Write(lowestByte);
                    shift = 7;
                    loopCount++;
                }
                while (value != 0);
            }

            public void Write7BitULong(ulong value)
            {
                byte lowestByte;
                byte mask = 0x7F; //
                int shift = 7;
                do
                {
                    lowestByte = (byte)(value & mask);
                    value >>= shift;
                    if (value != 0)
                        lowestByte |= 0x80;
                    Write(lowestByte);
                }
                while (value != 0);
            }

            public void WriteDateTime(DateTime dateTime)
            {
                Write(dateTime.Ticks);
            }

            //ticksResolution: e.g. TimeSpan.TicksPerSecond or TimeSpan.TicksPerhour
            public void WriteCompactDateTime(DateTime dateTime, long ticksResolution)
            {
                Write7BitLong(dateTime.Ticks / ticksResolution);
            }

            public void WriteTimeSpan(TimeSpan timeSpan)
            {
                Write7BitLong(timeSpan.Ticks);
            }

            public void WriteBytesArray(byte[] array)
            {
                if (array == null)
                {
                    Write7BitInt(0);
                }
                else
                {
                    Write7BitInt(array.Length);
                    Write(array);
                }
            }
        }
    }
}
