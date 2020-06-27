using Data;
using MongoWebApiStarter;
using MongoWebApiStarter.Auth;
using ServiceStack;

namespace Main.Account.Get
{
    public class Service : Service<Request, Response, Data.Account>
    {
        [Permission(Allow.Account_Read)]
        public override Response Get(Request r)
        {
            var acc = RepoAccount.FindExcluding(
                r.ID,
                a => new
                {
                    a.PasswordHash,
                    a.EmailVerificationCode
                });

            if (acc == null) throw HttpError.NotFound("Unable to find the right Account!");

            Response = ToResponse(acc);
            return Response;
        }

        protected override Response ToResponse(Data.Account a)
        {
            return new Response
            {
                City = a.Address.City,
                CountryCode = a.Address.CountryCode,
                EmailAddress = a.Email,
                FirstName = a.FirstName,
                ID = a.ID,
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
    }
}
