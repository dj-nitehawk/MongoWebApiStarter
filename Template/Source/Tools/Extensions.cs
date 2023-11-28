using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MongoWebApiStarter;

static class Extensions
{
    static readonly TextInfo _txt = new CultureInfo("en-US", false).TextInfo;

    /// <summary>
    /// Converts a string to TitleCase and trims whites space
    /// </summary>
    public static string TitleCase(this string value)
        => value.HasNoValue() ? value : _txt.ToTitleCase(value.Trim());

    /// <summary>
    /// Converts a string to lower-case and trims whites space
    /// </summary>
    public static string LowerCase(this string value)
        => value.HasNoValue() ? value : _txt.ToLower(value.Trim());

    /// <summary>
    /// Converts a string to UPPER-CASE and trims whites space
    /// </summary>
    public static string UpperCase(this string value)
        => value.HasNoValue() ? value : _txt.ToUpper(value.Trim());

    /// <summary>
    /// Not a null or empty string
    /// </summary>
    public static bool HasValue([AllowNull, NotNullWhen(true)] this string? value)
        => value != "null" && !string.IsNullOrEmpty(value);

    /// <summary>
    /// Is either null or an empty string
    /// </summary>
    public static bool HasNoValue([AllowNull, NotNullWhen(false)] this string? value)
        => value == "null" || string.IsNullOrEmpty(value);

    /// <summary>
    /// Convert a string in to a salted hash for storing
    /// </summary>
    public static string SaltedHash(this string value)
        => BCrypt.Net.BCrypt.HashPassword(value);

    /// <summary>
    /// The string meets the criteria for a strong password
    /// </summary>
    public static bool IsAValidPassword(this string password)
    {
        const int minLength = 8;
        const int maxLength = 20;

        if (string.IsNullOrEmpty(password))
            return false;

        var meetsLengthRequirements = password.Length is >= minLength and <= maxLength;
        var hasUpperCaseLetter = false;
        var hasLowerCaseLetter = false;
        var hasDecimalDigit = false;

        if (meetsLengthRequirements)
        {
            foreach (var c in password)
            {
                if (char.IsUpper(c))
                    hasUpperCaseLetter = true;
                else if (char.IsLower(c))
                    hasLowerCaseLetter = true;
                else if (char.IsDigit(c))
                    hasDecimalDigit = true;
            }
        }

        return meetsLengthRequirements &&
               hasUpperCaseLetter &&
               hasLowerCaseLetter &&
               hasDecimalDigit;
    }
}