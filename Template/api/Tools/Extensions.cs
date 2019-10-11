using Microsoft.AspNetCore.Mvc;

namespace MongoWebApiStarter.Api.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the base url for the current request
        /// </summary>
        /// <returns></returns>
        public static string BaseURL(this ControllerBase controller)
        {
            if (controller.Request != null)
            {
                return $"{controller.Request.Scheme}://{controller.Request.Host}/";
            }

            return "http://localhost:8888/";
        }

        /// <summary>
        /// Not a null or empty string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Is either a null or empty string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasNoValue(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
