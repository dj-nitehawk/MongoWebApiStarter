using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.Api.Controllers;
using System.Collections.Generic;
using System.Security.Claims;

namespace App.Test
{
    public static class Extensions
    {
        public static void SetClaims(this BaseController controller, IEnumerable<Claim> claims)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(claims))
                }
            };
        }
    }
}
