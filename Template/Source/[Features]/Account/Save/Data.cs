using MongoWebApiStarter;

namespace Account.Save;

static class Data
{
    public static async Task CreateOrUpdateAsync(Dom.Account acc)
    {
        using var tn = DB.Transaction();

        if (acc.ID.HasValue()) // existing account
            await tn.SavePreservingAsync(acc);
        else // new account
            await tn.SaveAsync(acc);

        await tn.CommitAsync();
    }

    public static Task SetEmailValidationCodeAsync(string code, string accoundID)
    {
        return DB.Update<Dom.Account>()
                 .Match(a => a.ID == accoundID)
                 .Modify(a => a.EmailVerificationCode, code)
                 .ExecuteAsync();
    }

    public static Task<string?> GetAccountIDAsync(string emailAddress)
    {
        return DB.Find<Dom.Account, string>()
                 .Match(a => a.Email == emailAddress.LowerCase())
                 .Project(a => a.ID)
                 .ExecuteSingleAsync();
    }
}