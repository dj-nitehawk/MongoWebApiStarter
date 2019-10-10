using MongoWebApiStarter.Api.Auth;
using MongoWebApiStarter.Biz.Settings;
using Microsoft.AspNetCore.Mvc;

namespace MongoWebApiStarter.Api.Base
{
    [NeedPermission]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public AppSettings Settings { get; set; }
    }
}