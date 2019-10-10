using Microsoft.AspNetCore.Mvc;

namespace MongoWebApiStarter.Api.Extensions
{
    public static class Extensions
    {
        public static string BaseURL(this ControllerBase controller)
        {
            if (controller.Request != null)
            {
                return $"{controller.Request.Scheme}://{controller.Request.Host}/";
            }

            return "http://localhost:8888/";
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool HasNoValue(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
