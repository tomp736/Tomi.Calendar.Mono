using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared;
using Tomi.Calendar.Mono.Shared.Dtos.DataTable;

namespace Tomi.Calendar.Mono.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DataTableController : ControllerBase
    {
        private readonly ILogger<DataTableController> _logger;

        public DataTableController(ILogger<DataTableController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ValueTask<RestDataTableResult> Post(RestDataTableRequest restDataTableRequest)
        {
            DataTable dt = DataTableHelper.GenerateTable(restDataTableRequest.TableName, restDataTableRequest.Columns, restDataTableRequest.Rows);
            RestDataTableResult dtResult = new RestDataTableResult()
            {
                Result = dt
            };
            return ValueTask.FromResult(dtResult);
        }
    }
}
