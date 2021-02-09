using ProtoBuf.Data;
using ProtoBuf.Grpc;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Tomi.Calendar.Proto
{
    [DataContract]
    public class DataTableRequest
    {
        [DataMember(Order = 1)]
        public string TableName { get; set; }
        [DataMember(Order = 2)]
        public int Columns { get; set; }
        [DataMember(Order = 3)]
        public int Rows { get; set; }
    }

    [DataContract]
    public class DataTableResult
    {
        [DataMember(Order = 1)]
        public DataTable DataTable { get; set; }
    }

    [ServiceContract(Name = "Mono.Calendar.DataTable")]
    public interface IDataTableService
    {
        ValueTask<DataTableResult> GetDataTable(DataTableRequest request, CallContext context = default);
    }

    [DataContract]
    public class DataTableSurrogate
    {
        [DataMember(Order = 1)]
        public byte[] DataTableBytes { get; set; }
        // protobuf-net wants an implicit or explicit operator between the types
        public static implicit operator DataTable(DataTableSurrogate value)
        {
            if (value == null)
                return null;

            DataTable dataTable = new DataTable();
            using (MemoryStream ms = new MemoryStream(value.DataTableBytes))
            {
                using (IDataReader reader = DataSerializer.Deserialize(ms))
                {
                    dataTable.Load(reader);
                }
                return dataTable;
            }

        }
        public static implicit operator DataTableSurrogate(DataTable value)
        {
            if (value == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (IDataReader reader = value.CreateDataReader())
                {
                    DataSerializer.Serialize(ms, reader);
                }
                return new DataTableSurrogate()
                {
                    DataTableBytes = ms.ToArray()
                };
            }
        }
    }
}
