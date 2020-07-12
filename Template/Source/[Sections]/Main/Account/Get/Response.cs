using MongoWebApiStarter;

namespace Main.Account.Get
{
    public class Response : Model, IResponse<Data.Account>
    {
        public bool IsEmailVerified { get; set; }

        public void FromEntity(Data.Account a)
        {
            City = a.Address.City;
            CountryCode = a.Address.CountryCode;
            EmailAddress = a.Email;
            FirstName = a.FirstName;
            ID = a.ID;
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
