using Microsoft.AspNetCore.Mvc;
using MongoWebApiStarter.Api.Auth;
using MongoWebApiStarter.Biz.Settings;

namespace MongoWebApiStarter.Api.Base
{
    /// <summary>
    /// Inherit this base class when creating new controller classes
    /// </summary>
    [NeedPermission]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public AppSettings Settings { get; set; }
    }
}