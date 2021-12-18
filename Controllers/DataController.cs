﻿using Microsoft.AspNetCore.Mvc;
using PGManagerApi.Models;
using PGManagerApi.Services;
using PGManagerApi.Authentication;
using System.Text.Json;

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
            var schemaTable = new Table(schema, table);

            return this.DataService.GetData(username, connection, schemaTable, startRow, rowCount);
        }

        [HttpPost("tables/{schema}/{table}/data")]
        public void InsertData([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table, [FromBody] Data data)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table(schema, table);

            this.DataService.InsertData(username, connection, schemaTable, data);
        }

        [HttpPut("tables/{schema}/{table}/data")]
        public void UpdateData([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table, [FromBody] Update update)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table(schema, table);

            this.DataService.UpdateData(username, connection, schemaTable, update);
        }

        [HttpDelete("tables/{schema}/{table}/data")]
        public void DeleteData([FromRoute] int connection, [FromRoute] string schema, [FromRoute] string table, [FromBody] Delete delete)
        {
            var username = this.HttpContext.User.Identity.Name;
            var schemaTable = new Table(schema, table);

            this.DataService.DeleteData(username, connection, schemaTable, delete);
        }
    }
}
