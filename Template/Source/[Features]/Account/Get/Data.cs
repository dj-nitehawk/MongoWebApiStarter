namespace Account.Get;

static class Data
{
    public static Task<Dom.Account?> GetAccountAsync(string accountID)
        => DB.Find<Dom.Account>()
             .Match(a => a.ID == accountID)
             .ProjectExcluding(
                 a => new {
                     a.PasswordHash,
                     a.EmailVerificationCode
                 })
             .ExecuteSingleAsync();
}