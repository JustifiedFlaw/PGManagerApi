using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PGManagerApi.Models;
using PGManagerApi.Services;
using PGManagerApi.Authentication;

namespace PGManagerApi.Controllers
{
    [ApiController]
    [Route("connections/{connection}")]
    [BasicAuthorization]
    public class SchemaController : ControllerBase
    {
        SchemaService SchemaService;

        public SchemaController(SchemaService schemaService)
        {
            this.SchemaService = schemaService;
        }

        [HttpGet("databases")]
        public IEnumerable<Database> GetDatabases([FromRoute] int connection)
        {
            var username = this.HttpContext.User.Identity.Name;
            return this.SchemaService.GetDatabases(username, connection);
        }

        [HttpPost("databases")]
        public void CreateDatabase([FromRoute] int connection, [FromBody] Database database)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.SchemaService.CreateDatabase(username, connection, database);
        }

        [HttpGet("tables")]
        public IEnumerable<Table> GetTables([FromRoute] int connection)
        {
            var username = this.HttpContext.User.Identity.Name;
            return this.SchemaService.GetTables(username, connection);
        }

        [HttpPost("tables")]
        public void CreateTable([FromRoute] int connection, [FromBody] Table table)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.SchemaService.CreateTable(username, connection, table);
        }

        [HttpPut("tables/{schema}/{table}")]
        public void RenameTable([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table, [FromQuery] string newName)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table
            {
                SchemaName = schema,
                TableName = table
            };
            this.SchemaService.RenameTable(username, connection, schemaTable, newName);
        }

        [HttpDelete("tables/{schema}/{table}")]
        public void DropTable([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table
            {
                SchemaName = schema,
                TableName = table
            };
            this.SchemaService.DropTable(username, connection, schemaTable);
        }

        [HttpGet("tables/{schema}/{table}/columns")]
        public IEnumerable<Column> GetColumns([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table
            {
                SchemaName = schema,
                TableName = table
            };
            return this.SchemaService.GetColumns(username, connection, schemaTable);
        }

        [HttpPost("tables/{schema}/{table}/columns")]
        public void AddColumns([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table, [FromBody] Column[] columns)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table
            {
                SchemaName = schema,
                TableName = table
            };
            this.SchemaService.AddColumns(username, connection, schemaTable, columns);
        }
    }
}
