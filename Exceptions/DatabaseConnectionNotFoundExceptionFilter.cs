using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PGManagerApi.Exceptions
{
    public class DatabaseConnectionNotFoundExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is DatabaseConnectionNotFoundException databaseConnectionNotFoundException)
            {
                context.Result = new ObjectResult(databaseConnectionNotFoundException.Message)
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };

                context.ExceptionHandled = true;
            }
        }
    }
}