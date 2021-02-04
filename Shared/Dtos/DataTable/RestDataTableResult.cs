using ProtoBuf.Data;
using System.Data;
using System.IO;
using System.Text.Json.Serialization;

namespace Tomi.Calendar.Mono.Shared.Dtos.DataTable
{
    public class RestDataTableResult
    {
        [JsonIgnore]
        public System.Data.DataTable Result
        {
            get
            {
                System.Data.DataTable dataTable = new System.Data.DataTable();
                using (MemoryStream ms = new MemoryStream(DataTableBytes))
                {
                    using (IDataReader reader = DataSerializer.Deserialize(ms))
                    {
                        dataTable.Load(reader);
                    }
                }
                return dataTable;
            }
            set
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (IDataReader reader = value.CreateDataReader())
                    {
                        DataSerializer.Serialize(ms, reader);
                    }
                    DataTableBytes = ms.ToArray();
                }
            }
        }

        public byte[] DataTableBytes { get; set; }
    }
}
