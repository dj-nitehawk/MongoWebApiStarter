using MongoDB.Entities;
using System;
using System.Text.RegularExpressions;

namespace MongoWebApiStarter.Biz
{
    /// <summary>
    /// Utility for working Google map links and embed codes
    /// </summary>
    public static class MapLocation
    {
        private static readonly Regex rxOne = new Regex("@(.*),(.*),", RegexOptions.Compiled); //map link
        private static readonly Regex rxTwo = new Regex(@"(?:2d|3d)([-\d].*?)!", RegexOptions.Compiled); //embed code

        /// <summary>
        /// Extracts lon+lat info from a google map link or embed code and returns a Coordinates2D object
        /// </summary>
        /// <param name="gMapLinkOrCode">Google map embed code or link</param>
        public static Coordinates2D Get2DCoordinates(string gMapLinkOrCode)
        {
            double lon, lat;

            if (gMapLinkOrCode.HasNoValue())
                return null;

            var match = rxOne.Match(gMapLinkOrCode);

            if (match.Success)
            {
                lon = Convert.ToDouble(match.Groups[2].Value); //lon is the second group
                lat = Convert.ToDouble(match.Groups[1].Value);
                return new Coordinates2D(lon, lat);
            }

            var matches = rxTwo.Matches(gMapLinkOrCode);

            if (matches.Count > 0)
            {
                lon = Convert.ToDouble(matches[0].Groups[1].Value); //lon is the first match group
                lat = Convert.ToDouble(matches[1].Groups[1].Value);
                return new Coordinates2D(lon, lat);
            }

            return null;
        }

        /// <summary>
        /// Validates a google map embed code or map link
        /// </summary>
        /// <param name="gMapLinkOrCode">Google map embed code or link</param>
        public static bool IsValid(string gMapLinkOrCode)
        {
            if (gMapLinkOrCode.HasNoValue()) return true;

            return Get2DCoordinates(gMapLinkOrCode) != null;
        }
    }
}
