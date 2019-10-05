using Microsoft.AspNetCore.Authorization;
using App.Biz;
using App.Biz.Models;
using App.Biz.Views;

namespace App.Api.Auth
{
    public static class Authorization
    {
        public static void EnableAppPolicies(this AuthorizationOptions opts)
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
        }
    }
}
