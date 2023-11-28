using System.Text.RegularExpressions;

namespace MongoWebApiStarter;

/// <summary>
/// Utility for working Google map links and embed codes
/// </summary>
static class MapLocation
{
    static readonly Regex _rxOne = new("@(.*),(.*),", RegexOptions.Compiled);           //map link
    static readonly Regex _rxTwo = new(@"(?:2d|3d)([-\d].*?)!", RegexOptions.Compiled); //extract lon+lat from embed code
    static readonly Regex _rxThree = new(@"src=""(.+?)""", RegexOptions.Compiled);      //extract url from embed code

    /// <summary>
    /// Extracts lon+lat info from a google map link or embed code and returns a Coordinates2D object
    /// </summary>
    /// <param name="gMapLinkOrCode">Google map embed code or link</param>
    public static Coordinates2D Get2DCoordinates(string gMapLinkOrCode)
    {
        double lon,
               lat;

        if (gMapLinkOrCode.HasNoValue())
            throw new ArgumentException("Supplied gmap link or code is empty!");

        var match = _rxOne.Match(gMapLinkOrCode);

        if (match.Success)
        {
            lon = Convert.ToDouble(match.Groups[2].Value); //lon is the second group
            lat = Convert.ToDouble(match.Groups[1].Value);

            return new(lon, lat);
        }

        var matches = _rxTwo.Matches(gMapLinkOrCode);

        if (matches.Count > 0)
        {
            lon = Convert.ToDouble(matches[0].Groups[1].Value); //lon is the first match group
            lat = Convert.ToDouble(matches[1].Groups[1].Value);

            return new(lon, lat);
        }

        throw new InvalidOperationException("Unable to extract 2d coordinates from the supplied gmap link or code!");
    }

    /// <summary>
    /// Validates a google map embed code or map link
    /// </summary>
    /// <param name="gMapLinkOrCode">Google map embed code or link</param>
    public static bool IsValid(string gMapLinkOrCode)
        => gMapLinkOrCode.HasNoValue() || Get2DCoordinates(gMapLinkOrCode).Coordinates.Length == 2;

    /// <summary>
    /// Extracts the URL from Google Map Embed code
    /// </summary>
    /// <param name="gMapEmbedCode">The Google Map Embed code</param>
    public static string EmbedCodeToURL(string gMapEmbedCode)
    {
        if (gMapEmbedCode.HasNoValue())
            return gMapEmbedCode;

        var match = _rxThree.Match(gMapEmbedCode);

        return match.Success
                   ? match.Groups[1].Value
                   : gMapEmbedCode;
    }

    /// <summary>
    /// Returns true if the supplied link is a Google Map Embed URL
    /// </summary>
    /// <param name="link">The URL to check</param>
    public static bool IsEmbedLink(string link)
        => link.HasNoValue() || link.StartsWith("https://www.google.com/maps/embed");
}