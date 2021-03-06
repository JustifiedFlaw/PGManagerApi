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

        [HttpGet("schemas")]
        public IEnumerable<Schema> GetSchemas([FromRoute] int connection)
        {
            var username = this.HttpContext.User.Identity.Name;
            return this.SchemaService.GetSchemas(username, connection);
        }

        [HttpPost("schemas/{schemaName}")]
        public void CreateSchema([FromRoute] int connection, [FromRoute] string schemaName)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.SchemaService.CreateSchema(username, connection, schemaName);
        }

        [HttpPut("schemas/{schemaName}")]
        public void RenameSchema([FromRoute] int connection, [FromRoute] string schemaName, [FromQuery] string newName)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.SchemaService.RenameSchema(username, connection, schemaName, newName);
        }

        [HttpDelete("schemas/{schemaName}")]
        public void DropSchema([FromRoute] int connection, [FromRoute] string schemaName)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.SchemaService.DropSchema(username, connection, schemaName);
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
            var schemaTable = new Table(schema, table);

            this.SchemaService.RenameTable(username, connection, schemaTable, newName);
        }

        [HttpDelete("tables/{schema}/{table}")]
        public void DropTable([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table(schema, table);

            this.SchemaService.DropTable(username, connection, schemaTable);
        }

        [HttpGet("tables/{schema}/{table}/columns")]
        public IEnumerable<Column> GetColumns([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table(schema, table);

            return this.SchemaService.GetColumns(username, connection, schemaTable);
        }

        [HttpPost("tables/{schema}/{table}/columns")]
        public void AddColumns([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table, [FromBody] Column[] columns)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table(schema, table);

            this.SchemaService.AddColumns(username, connection, schemaTable, columns);
        }

        [HttpPut("tables/{schema}/{table}/columns/{columnName}")]
        public void RenameColumn([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table, [FromRoute] string columnName, [FromQuery] string newName)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table(schema, table);

            this.SchemaService.RenameColumn(username, connection, schemaTable, columnName, newName);
        }

        [HttpDelete("tables/{schema}/{table}/columns/{columnName}")]
        public void DropColumn([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table, [FromRoute] string columnName)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table(schema, table);
            
            this.SchemaService.DropColumn(username, connection, schemaTable, columnName);
        }

        [HttpGet("tables/{schema}/{table}/primarykey")]
        public IEnumerable<string> GetPrimaryKey([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table(schema, table);
            
            return this.SchemaService.GetPrimaryKey(username, connection, schemaTable);
        }
    }
}
