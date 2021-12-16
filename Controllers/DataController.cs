using Microsoft.AspNetCore.Mvc;
using PGManagerApi.Models;
using PGManagerApi.Services;
using PGManagerApi.Authentication;

namespace PGManagerApi.Controllers
{
    [ApiController]
    [Route("connections/{connection}")]
    [BasicAuthorization]
    public class DataController : ControllerBase
    {
        DataService DataService;

        public DataController(DataService dataService)
        {
            this.DataService = dataService;
        }

        [HttpGet("tables/{schema}/{table}/data")]
        public Data GetData([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table, [FromQuery] int startRow = 0, [FromQuery] int rowCount = 100)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table
            {
                SchemaName = schema,
                TableName = table
            };
            return this.DataService.GetData(username, connection, schemaTable, startRow, rowCount);
        }
    }
}
