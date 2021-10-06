using FastEndpoints;
using MongoWebApiStarter.Auth;

namespace Account.Get
{
    public class Request
    {
        [From(Claim.AccountID, IsRequired = true)]
        public string AccountID { get; set; }
    }

    public class Response : Model, IResponse<Dom.Account>
    {
        public string AccountID { get; set; }
        public bool IsEmailVerified { get; set; }

        public void FromEntity(Dom.Account a)
        {
            AccountID = a.ID;
            City = a.Address.City;
            CountryCode = a.Address.CountryCode;
            EmailAddress = a.Email;
            FirstName = a.FirstName;
            IsEmailVerified = a.IsEmailVerified;
            LastName = a.LastName;
            Mobile = a.Mobile;
            Password = "";
            State = a.Address.State;
            Street = a.Address.Street;
            Title = a.Title;
            ZipCode = a.Address.ZipCode;
        }
    }
}
