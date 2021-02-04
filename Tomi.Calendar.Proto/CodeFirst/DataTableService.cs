using ProtoBuf.Data;
using ProtoBuf.Grpc;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Tomi.Calendar.Proto.CodeFirst
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
        public DataTable Result
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

        [DataMember(Order = 1)]
        public byte[] DataTableBytes { get; set; }
    }

    [ServiceContract(Name = "Mono.Calendar.DataTable")]
    public interface IDataTableService
    {
        ValueTask<DataTableResult> GetDataTable(DataTableRequest request, CallContext context = default);
    }
}
