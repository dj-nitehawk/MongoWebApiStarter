using App.Biz.Models;
using App.Biz.Views;
using Microsoft.AspNetCore.Authorization;

namespace App.Biz.Auth
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
        }
    }
}
