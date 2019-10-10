namespace MongoWebApiStarter.Biz
{
    public static class Extensions
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool HasNoValue(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string ToSaltedHash(this string value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value);
        }

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

            bool isValid = meetsLengthRequirements
                        && hasUpperCaseLetter
                        && hasLowerCaseLetter
                        && hasDecimalDigit
                        ;
            return isValid;
        }
    }


}
