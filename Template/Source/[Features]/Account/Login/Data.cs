using MongoWebApiStarter;

namespace Account.Login;

internal static class Data
{
    public static Task<Dom.Account?> GetAccountAsync(string userName)
    {
        return DB.Find<Dom.Account>()
                 .Match(a => a.Email == userName.LowerCase())
                 .Project(a => new()
                 {
                     PasswordHash = a.PasswordHash,
                     IsEmailVerified = a.IsEmailVerified,
                     ID = a.ID,
                     Title = a.Title,
                     FirstName = a.FirstName,
                     LastName = a.LastName
                 })
                 .ExecuteSingleAsync();
    }
}