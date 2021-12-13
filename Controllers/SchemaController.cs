using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PGManagerApi.Models;
using PGManagerApi.Services;
using PGManagerApi.Authentication;

namespace PGManagerApi.Controllers
{
    [ApiController]
    [Route("{database}")]
    [BasicAuthorization]
    public class SchemaController : ControllerBase
    {
        SchemaService SchemaService;

        public SchemaController(SchemaService schemaService)
        {
            this.SchemaService = schemaService;
        }

        [HttpGet("tables")]
        public IEnumerable<Table> GetTables([FromRoute] string database)
        {
            var username = this.HttpContext.User.Identity.Name;
            return this.SchemaService.GetTables(username, database);
        }
    }
}
