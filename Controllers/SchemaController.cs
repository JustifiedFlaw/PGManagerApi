using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PGManagerApi.Models;
using PGManagerApi.Services;

namespace PGManagerApi.Controllers
{
    [ApiController]
    [Route("")]
    public class SchemaController : ControllerBase
    {
        SchemaService SchemaService;

        public SchemaController(SchemaService schemaService)
        {
            this.SchemaService = schemaService;
        }

        [HttpGet("tables")]
        public IEnumerable<Table> GetTables()
        {
            // TODO get user and db by auth or session or something
            return this.SchemaService.GetTables("JustifiedFlaw", "local");
        }
    }
}
