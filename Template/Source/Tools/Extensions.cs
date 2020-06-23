using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SCVault
{
    public static class Extensions
    {
        private static readonly TextInfo txt = new CultureInfo("en-US", false).TextInfo;

        /// <summary>
        /// Converts a string to TitleCase and trims whites space
        /// </summary>
        public static string TitleCase(this string value)
        {
            if (value.HasNoValue()) return value;
            return txt.ToTitleCase(value.Trim());
        }

        /// <summary>
        /// Converts a string to lower-case and trims whites space
        /// </summary>
        public static string LowerCase(this string value)
        {
            if (value.HasNoValue()) return value;
            return txt.ToLower(value.Trim());
        }

        /// <summary>
        /// Converts a string to UPPER-CASE and trims whites space
        /// </summary>
        public static string UpperCase(this string value)
        {
            if (value.HasNoValue()) return value;
            return txt.ToUpper(value.Trim());
        }

        /// <summary>
        /// Not a null or empty string
        /// </summary>
        public static bool HasValue(this string value)
        {
            return value != "null" && !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Is either null or an empty string
        /// </summary>
        public static bool HasNoValue(this string value)
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

            bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
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

        /// <summary>
        /// Returns an IEnumerable of enum values
        /// </summary>
        public static IEnumerable<T> All<T>(this T _) where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Returns a dictionary of enum name & enum value
        /// </summary>
        public static Dictionary<string, T> ToDictionary<T>(this T _) where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(i => i.ToString(), i => i);
        }
    }
}
