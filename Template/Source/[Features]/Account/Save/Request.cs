using Dom;
using MongoWebApiStarter;
using ServiceStack;

namespace Account.Save
{
    [Route("/account")]
    public class Request : Model, IRequest<Dom.Account, Response>
    {
        public Dom.Account ToEntity()
        {
            return new Dom.Account()
            {
                ID = ID,
                Email = EmailAddress.LowerCase(),
                PasswordHash = Password.SaltedHash(),
                Title = Title,
                FirstName = FirstName.TitleCase(),
                LastName = LastName.TitleCase(),
                Address = new Address
                {
                    Street = Street.TitleCase(),
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
