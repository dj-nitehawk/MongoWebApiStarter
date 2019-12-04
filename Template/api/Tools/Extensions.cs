namespace MongoWebApiStarter.Api.Extensions
{
    public static class Extensions
    {
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
