using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MongoWebApiStarter;

public static class Extensions
{
    private static readonly TextInfo txt = new CultureInfo("en-US", false).TextInfo;

    /// <summary>
    /// Converts a string to TitleCase and trims whites space
    /// </summary>
    public static string TitleCase(this string value)
    {
        return value.HasNoValue() ? value : txt.ToTitleCase(value.Trim());
    }

    /// <summary>
    /// Converts a string to lower-case and trims whites space
    /// </summary>
    public static string LowerCase(this string value)
    {
        return value.HasNoValue() ? value : txt.ToLower(value.Trim());
    }

    /// <summary>
    /// Converts a string to UPPER-CASE and trims whites space
    /// </summary>
    public static string UpperCase(this string value)
    {
        return value.HasNoValue() ? value : txt.ToUpper(value.Trim());
    }

    /// <summary>
    /// Not a null or empty string
    /// </summary>
    public static bool HasValue([AllowNull, NotNullWhen(true)] this string? value)
    {
        return value != "null" && !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Is either null or an empty string
    /// </summary>
    public static bool HasNoValue([AllowNull, NotNullWhen(false)] this string? value)
    {
        return value == "null" || string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Convert a string in to a salted hash for storing
    /// </summary>
    public static string SaltedHash(this string value)
    {
        return BCrypt.Net.BCrypt.HashPassword(value);
    }

    /// <summary>
    /// The string meets the criteria for a strong password
    /// </summary>
    public static bool IsAValidPassword(this string password)
    {
        const int MIN_LENGTH = 8;
        const int MAX_LENGTH = 20;

        if (password == null) return false;

        bool meetsLengthRequirements = password.Length is >= MIN_LENGTH and <= MAX_LENGTH;
        bool hasUpperCaseLetter = false;
        bool hasLowerCaseLetter = false;
        bool hasDecimalDigit = false;

        if (meetsLengthRequirements)
        {
            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpperCaseLetter = true;
                else if (char.IsLower(c)) hasLowerCaseLetter = true;
                else if (char.IsDigit(c)) hasDecimalDigit = true;
            }
        }

        return meetsLengthRequirements &&
               hasUpperCaseLetter &&
               hasLowerCaseLetter &&
               hasDecimalDigit;
    }
}

