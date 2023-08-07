namespace Account.Verify;

internal static class Data
{
    public static async Task<bool> ValidateEmailAsync(string accountID, string code)
    {
        var accExists = await DB.Find<Dom.Account>()
                                .Match(a => a.ID == accountID && a.EmailVerificationCode == code)
                                .ExecuteAnyAsync();

        if (!accExists) return false;

        await DB.Update<Dom.Account>()
                .Match(a => a.ID == accountID)
                .Modify(a => a.IsEmailVerified, true)
                .ExecuteAsync();

        return true;
    }
}