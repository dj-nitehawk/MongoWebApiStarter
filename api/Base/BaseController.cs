using App.Api.Auth;
using App.Biz.Settings;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Base
{
    [NeedPermission]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public AppSettings Settings { get; set; }
    }
}