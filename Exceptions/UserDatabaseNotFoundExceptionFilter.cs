using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PGManagerApi.Exceptions
{
    public class UserDatabaseNotFoundExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is UserDatabaseNotFoundException userDatabaseNotFoundException)
            {
                context.Result = new ObjectResult(userDatabaseNotFoundException.Message)
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };

                context.ExceptionHandled = true;
            }
        }
    }
}