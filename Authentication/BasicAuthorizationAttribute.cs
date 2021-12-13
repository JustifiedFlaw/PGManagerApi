using Microsoft.AspNetCore.Authorization;

namespace PGManagerApi.Authentication
{      
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {
        public BasicAuthorizationAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }
}