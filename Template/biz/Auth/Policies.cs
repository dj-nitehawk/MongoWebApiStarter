using Microsoft.AspNetCore.Authorization;
using MongoWebApiStarter.Biz.Models;
using MongoWebApiStarter.Biz.Views;
using System;

namespace MongoWebApiStarter.Biz.Auth
{
    public static class Policies
    {
        public static void EnablePolicies(this AuthorizationOptions opts)
        {
            // account holders can save account
            opts.AddPolicy(AccountModel.Perms.Save,
                           b => b.RequireClaim(AccountModel.Claims.ID));

            // account holders can save images
            opts.AddPolicy(ImageModel.Perms.Save,
                           b => b.RequireClaim(AccountModel.Claims.ID));

            // admins can view account list
            opts.AddPolicy(AccountsView.Perms.View,
                           b => b.RequireRole(Roles.Admin));

            // gmail users can view account list regardless of role
            opts.AddPolicy(AccountsView.Perms.View,
                           b => b.RequireAssertion(UserHasGmailAddress));

        }

        // CUSTOM REQUIREMENTS:        
        static Func<AuthorizationHandlerContext, bool> UserHasGmailAddress =
            ctx =>
            {
                var email = ctx.User.FindFirst(AccountModel.Claims.Email)?.Value;
                return email == null ?
                       false :
                       email.Split('@')[1].Equals("gmail.com");
            };

    }
}
