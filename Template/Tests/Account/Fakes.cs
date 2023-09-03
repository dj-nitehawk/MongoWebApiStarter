using Bogus;
using Save = Account.Save;

namespace Tests.Account;

static class Fakes
{
    public static Save.Request SaveRequest(this Faker f) => new()
    {
        City = f.Address.City(),
        CountryCode = f.Address.County(),
        EmailAddress = f.Internet.Email(),
        FirstName = f.Name.FirstName(),
        LastName = f.Name.LastName(),
        Mobile = f.Phone.PhoneNumber("##########"),
        Password = f.Internet.Password(10),
        State = f.Address.State(),
        Street = f.Address.StreetName(),
        Title = f.Name.Prefix(),
        ZipCode = f.Address.ZipCode(),
    };
}
