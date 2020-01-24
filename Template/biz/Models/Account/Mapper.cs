using MongoWebApiStarter.Biz.Interfaces;
using MongoWebApiStarter.Data.Entities;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class AccountModel : IMapper<Account>
    {
        public void LoadFrom(Account a)
        {
            EmailAddress = a.Email;
            Title = a.Title;
            FirstName = a.FirstName;
            LastName = a.LastName;
            Street = a.Address.Street;
            City = a.Address.City;
            State = a.Address.State;
            ZipCode = a.Address.ZipCode;
            CountryCode = a.Address.CountryCode;
            IsEmailVerified = a.IsEmailVerified;
        }

        public Account ToEntity()
        {
            return new Account
            {
                ID = ID,
                Email = EmailAddress.ToLower(),
                PasswordHash = Password.ToSaltedHash(),
                Title = Title,
                FirstName = FirstName,
                LastName = LastName,
                Address = new Address
                {
                    Street = Street,
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    CountryCode = CountryCode
                },
                IsEmailVerified = IsEmailVerified
            };
        }
    }
}
