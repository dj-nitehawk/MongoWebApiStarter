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
        public AppSettings Settings
        {
            get
            {
                if (Request == null || Request.HttpContext == null || Request.HttpContext.RequestServices == null)
                {
                    return new AppSettings();
                }

                return Request.HttpContext.RequestServices.GetService(typeof(AppSettings)) as AppSettings;
            }
        }

        public string BaseURL
        {
            get
            {
                if (Request != null)
                {
                    return $"{Request.Scheme}://{Request.Host}/";
                }

                return "http://localhost:8888/";
            }
        }
    }
}