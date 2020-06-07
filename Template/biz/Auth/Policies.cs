using Microsoft.AspNetCore.Authorization;
using MongoWebApiStarter.Biz.Models;
using MongoWebApiStarter.Biz.Views;
using System;

namespace MongoWebApiStarter.Biz.Auth
{
    public static class Policies
    {
        public static void EnablePolicies(this AuthorizationOptions x)
        {
            #region Account
            //read
            x.AddPolicy(
                AccountModel.Perms.Read,
                b => b.RequireClaim(AccountModel.Claims.ID)
                      .RequireRole(
                        Roles.Admin,
                        Roles.Owner));
            //write
            x.AddPolicy(
                AccountModel.Perms.Write,
                b => b.RequireClaim(AccountModel.Claims.ID)
                      .RequireRole(
                        Roles.Admin,
                        Roles.Owner));
            //delete
            x.AddPolicy(
                AccountModel.Perms.Delete,
                b => b.RequireClaim(AccountModel.Claims.ID)
                      .RequireRole(Roles.Admin));
            #endregion

            #region Image
            //read
            x.AddPolicy(
                ImageModel.Perms.Read,
                b => b.RequireAuthenticatedUser());
            //write
            x.AddPolicy(
                ImageModel.Perms.Write,
                b => b.RequireAuthenticatedUser());
            //delete
            x.AddPolicy(
                ImageModel.Perms.Delete,
                b => b.RequireAuthenticatedUser());
            #endregion

            #region AccountList
            //read
            x.AddPolicy(
                AccountsView.Perms.Read,
                b => b.RequireClaim(AccountModel.Claims.ID)
                      .RequireRole(
                        Roles.Admin,
                        Roles.Owner));
            //custom
            x.AddPolicy(
                AccountsView.Perms.Read,
                b => b.RequireAssertion(UserHasGmailAddress));
            #endregion       
        }

        #region CUSTOM REQUIREMENTS

        private static readonly Func<AuthorizationHandlerContext, bool> UserHasGmailAddress =
            ctx =>
            {
                var email = ctx.User.FindFirst(AccountModel.Claims.Email)?.Value;
                return email != null && email.Split('@')[1].Equals("gmail.com");
            };

        #endregion
    }
}
