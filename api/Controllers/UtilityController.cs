using App.Api.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    public class UtilityController : BaseController
    {
        private readonly IWebHostEnvironment env;

        public UtilityController(IWebHostEnvironment environment)
        {
            env = environment;
        }

        [AllowAnonymous]
        [HttpGet("show-log")] // don't forget to lock this url down with nginx
        public ActionResult ShowLog()
        {
            if (System.IO.File.Exists("output.log"))
            {
                var file = System.IO.Path.Combine(env.ContentRootPath, "output.log");
                return PhysicalFile(file, "text/plain");
            }

            return Ok("no log output yet...");
        }
    }
}
