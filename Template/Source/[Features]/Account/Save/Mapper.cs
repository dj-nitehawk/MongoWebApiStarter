using Dom;
using MongoWebApiStarter;

namespace Account.Save;

internal sealed class Mapper : Mapper<Request, Response, Dom.Account>
{
    public override Dom.Account ToEntity(Request r) => new()
    {
        ID = r.AccountID,
        Email = r.EmailAddress.LowerCase(),
        PasswordHash = r.Password.SaltedHash(),
        Title = r.Title,
        FirstName = r.FirstName.TitleCase(),
        LastName = r.LastName.TitleCase(),
        Address = new Address
        {
            Street = r.Street.TitleCase(),
            City = r.City,
            State = r.State,
            ZipCode = r.ZipCode,
            CountryCode = r.CountryCode
        },
        Mobile = r.Mobile,
    };
}
