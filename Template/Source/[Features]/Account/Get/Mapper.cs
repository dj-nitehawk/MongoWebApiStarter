namespace Account.Get;

internal sealed class Mapper : Mapper<Request, Response, Dom.Account>
{
    public override Response FromEntity(Dom.Account a) => new()
    {
        AccountID = a.ID!,
        City = a.Address.City,
        CountryCode = a.Address.CountryCode,
        EmailAddress = a.Email,
        FirstName = a.FirstName,
        IsEmailVerified = a.IsEmailVerified,
        LastName = a.LastName,
        Mobile = a.Mobile,
        Password = "",
        State = a.Address.State,
        Street = a.Address.Street,
        Title = a.Title,
        ZipCode = a.Address.ZipCode
    };
}
