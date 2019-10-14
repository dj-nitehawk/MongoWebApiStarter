using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoWebApiStarter.Api.Auth;
using MongoWebApiStarter.Api.Base;
using System;
using System.Security.Claims;

namespace SCVault.Test
{
    public static class Extensions
    {
        public static void SetClaims(this BaseController controller, Action<ClaimBuilder> action)
        {
            var builder = new ClaimBuilder();
            action(builder);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(builder.GetClaims()))
                }
            };
        }
    }
}