using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PGManagerApi.Models;
using PGManagerApi.Services;
using PGManagerApi.Authentication;

namespace PGManagerApi.Controllers
{
    [ApiController]
    [Route("connections")]
    [BasicAuthorization]
    public class ConnectionController : ControllerBase
    {
        DatabaseConnectionService DatabaseConnectionService;

        public ConnectionController(DatabaseConnectionService databaseConnectionService)
        {
            this.DatabaseConnectionService = databaseConnectionService;
        }

        [HttpGet()]
        public IEnumerable<DatabaseConnection> GetConnections()
        {
            var username = this.HttpContext.User.Identity.Name;
            return this.DatabaseConnectionService.GetConnections(username);
        }

        [HttpGet("{connection}")]
        public DatabaseConnection GetConnection([FromRoute] string connection)
        {
            var username = this.HttpContext.User.Identity.Name;
            return this.DatabaseConnectionService.GetConnection(username, connection);
        }

        [HttpPut]
        public void Update([FromBody] DatabaseConnection connection)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.DatabaseConnectionService.Update(connection);
        }

        [HttpPost]
        public void Add([FromBody] DatabaseConnection connection)
        {
            var username = this.HttpContext.User.Identity.Name;
            this.DatabaseConnectionService.Add(connection);
        }
    }
}
