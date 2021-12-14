﻿using System.Collections.Generic;
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
        public IEnumerable<Database> GetDatabases([FromRoute] string connection)
        {
            var username = this.HttpContext.User.Identity.Name;
            return this.SchemaService.GetDatabases(username, connection);
        }

        [HttpPost("databases")]
        public void CreateDatabase([FromRoute] string connection, [FromBody] Database database)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.SchemaService.CreateDatabase(username, connection, database);
        }

        [HttpGet("tables")]
        public IEnumerable<Table> GetTables([FromRoute] string connection)
        {
            var username = this.HttpContext.User.Identity.Name;
            return this.SchemaService.GetTables(username, connection);
        }

        [HttpPost("tables")]
        public void CreateTable([FromRoute] string connection, [FromBody] Table table)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.SchemaService.CreateTable(username, connection, table);
        }

        [HttpDelete("tables")]
        public void DropTable([FromRoute] string connection, [FromBody] Table table)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.SchemaService.CreateTable(username, connection, table);
        }

        [HttpGet("tables/{schema}/{table}/columns")]
        public IEnumerable<Column> GetColumns([FromRoute] string connection, [FromRoute] string schema, [FromRoute] string table)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table
            {
                SchemaName = schema,
                TableName = table
            };
            return this.SchemaService.GetColumns(username, connection, schemaTable);
        }
    }
}
