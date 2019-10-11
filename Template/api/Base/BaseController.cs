using MongoWebApiStarter.Api.Auth;
using MongoWebApiStarter.Biz.Settings;
using Microsoft.AspNetCore.Mvc;

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