﻿using MongoWebApiStarter;
using MongoWebApiStarter.Auth;
using ServiceStack;
using System.Linq;

namespace Main.Account.Login
{
    [Authenticate(ApplyTo.None)]
    public class Service : Service<Request, Response, Database>
    {
        public Response Post(Request r)
        {
            var acc = Data.GetAccount(r.UserName);

            if (acc != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(r.Password, acc.PasswordHash))
                    ThrowError("The supplied credentials are invalid. Please try again...");
            }
            else
            {
                ThrowError("Sorry, couldn't locate your account...");
            }

            if (!acc.IsEmailVerified)
                ThrowError("Please verify your email address before logging in...");

            var permissions = default(Allow).All().ToArray();

            var session = new UserSession((Claim.AccountID, acc.ID));

            Response.SignIn(session, permissions);
            Response.FullName = $"{acc.Title} {acc.FirstName} {acc.LastName}";

            return Response;
        }
    }
}
