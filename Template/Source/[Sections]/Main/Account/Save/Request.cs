using Data;
using MongoWebApiStarter;
using ServiceStack;

namespace Main.Account.Save
{
    [Route("/account")]
    public class Request : Model, IRequest<Data.Account, Response>
    {
        public Data.Account ToEntity()
        {
            return new Data.Account()
            {
                ID = ID,
                Email = EmailAddress.LowerCase(),
                PasswordHash = Password.SaltedHash(),
                Title = Title,
                FirstName = FirstName.TitleCase(),
                LastName = LastName.TitleCase(),
                Address = new Address
                {
                    Street = Street.ToTitleCase(),
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    CountryCode = CountryCode
                },
                Mobile = Mobile,
            };
        }
    }
}
