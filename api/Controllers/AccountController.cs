using App.Api.Auth;
using App.Biz.Auth;
using App.Api.Extensions;
using App.Biz.Models;
using App.Biz.Settings;
using App.Biz.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Api.Controllers
{
    [NeedPermission(AccountModel.Perms.Save)]
    public class AccountController : BaseController
    {
        public AccountController(AppSettings settings)
        {
            Settings = settings;
        }

        [AllowAnonymous]
        [HttpPost("api/account")]
        public ActionResult Save(AccountModel model)
        {
            model.Save();
            model.SendVerificationEmail(this.BaseURL(), Settings.Email.FromName, Settings.Email.FromEmail);
            return Ok(new
            {
                model.ID,
                EmailSent = model.NeedsEmailVerification
            });
        }

        [HttpPatch("api/account")]
        public ActionResult Update(AccountModel model)
        {
            model.ID = User.FindFirstValue(AccountModel.Claims.ID); //ignore client submitted id and read from token to prevent post tampering

            return Save(model);
        }

        [HttpGet("api/account/{id}")]
        public ActionResult<AccountModel> Retrieve(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound("Invalid ID");

            var model = new AccountModel { ID = id };
            model.Load();
            return model;
        }

        [AllowAnonymous]
        [HttpGet("/api/account/{id}-{code}/validate")]
        public ActionResult ValidateEmail(string id, string code)
        {
            var model = new AccountModel() { ID = id };

            if (model.ValidateEmailAddress(code))
            {
                return Ok();
            }

            return BadRequest("Sorry! Could not validate your email address...");
        }

        [AllowAnonymous]
        [HttpPost("/api/account/login")]
        public ActionResult Login(LoginModel model)
        {
            var error = model.SingIn();

            if (error.IsNotEmpty())
            {
                return BadRequest(error);
            }

            var claims = new[] {
                    new Claim(AccountModel.Claims.ID, model.AccountID),
                    new Claim(Claims.Role, Roles.Owner)
                };

            return Ok(new
            {
                Token = Authentication.GenerateToken(claims)
            });
        }

        [NeedPermission(AccountsView.Perms.View)]
        [HttpGet("/api/view/accounts")]
        public ActionResult<AccountsView> ViewAccounts()
        {
            return Ok(new AccountsView().Load());
        }
    }
}
