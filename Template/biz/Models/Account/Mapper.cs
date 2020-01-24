using MongoWebApiStarter.Biz.Interfaces;
using MongoWebApiStarter.Data.Entities;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class AccountModel : IMapper<AccountModel, Account>
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

        public Account ToEntity(AccountModel m)
        {
            return new Account
            {
                ID = m.ID,
                Email = m.EmailAddress.ToLower(),
                PasswordHash = m.Password.ToSaltedHash(),
                Title = m.Title,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Address = new Address
                {
                    Street = m.Street,
                    City = m.City,
                    State = m.State,
                    ZipCode = m.ZipCode,
                    CountryCode = m.CountryCode
                },
                IsEmailVerified = m.IsEmailVerified
            };
        }
    }
}
