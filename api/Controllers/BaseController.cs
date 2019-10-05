using Microsoft.AspNetCore.Mvc;
using App.Api.Auth;
using App.Biz.Settings;

namespace App.Api.Controllers
{
    [NeedPermission]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public AppSettings Settings { get; set; }
    }
}