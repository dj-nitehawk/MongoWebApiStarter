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
            // account holders have full access to accounts
            opts.AddPolicy(AccountModel.Perms.Full,
                           b => b.RequireClaim(AccountModel.Claims.ID));

            // account holders have full access to images
            opts.AddPolicy(ImageModel.Perms.Full,
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
