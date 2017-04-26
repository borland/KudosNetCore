using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Newtonsoft.Json;
using KudosNetCore.Model;
using System.Security.Principal;

namespace KudosNetCore
{
    public class AuthenticatedUserAttribute : ActionFilterAttribute
    {
        class UserIdentity : IIdentity
        {
            public UserIdentity(User user) => User = user;

            public User User { get; private set; }

            public string AuthenticationType => "UserId";

            public bool IsAuthenticated => true;

            public string Name => User.FullName ?? User.Username;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Cookies is IRequestCookieCollection cookies) {
                if (cookies["auth"] is string auth && int.TryParse(auth, out var userId)) {

                    var repo = (Repository)context.HttpContext.RequestServices.GetService(typeof(Repository));
                    if(repo.Users.FirstOrDefault(u => u.Id == userId) is User user) {
                        context.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(new UserIdentity(user));
                        return; // all good
                    }
                }
            }
            // else bail out
            context.Result = new RedirectToRouteResult(new { controller = "Users", action = "Login" });
        }


        protected Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            if(context.Resource is AuthorizationFilterContext ctx) {
                
            }
            
            return Task.FromResult(true);
        }
    }

    // experimental JWT stuff for some point in the future

    public struct JwtHeader
    {
        [JsonProperty("alg")]
        public string Alg { get; set; }
        [JsonProperty("typ")]
        public string Typ { get; set; }

        public JwtHeader(string alg, string typ)
        {
            Alg = alg;
            Typ = typ;
        }

        public static JwtHeader HS256 => new JwtHeader("HS256", "JWT");
    }

    public struct AuthPayload
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
    }

    public class JWT
    {
        private static readonly byte[] HMACSecret = Convert.FromBase64String("DWS08ZZfpnaX5BPBYt6gVHrqketrzJ3s/nPlsprAxak="); // todo put this in a config, not in source control

        
    }
}
