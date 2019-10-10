using MongoWebApiStarter.Api.Auth;
using MongoWebApiStarter.Api.Base;
using MongoWebApiStarter.Api.Extensions;
using MongoWebApiStarter.Biz.Auth;
using MongoWebApiStarter.Biz.Models;
using MongoWebApiStarter.Biz.Settings;
using MongoWebApiStarter.Biz.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MongoWebApiStarter.Api.Controllers
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
            model.SendVerificationEmail(this.BaseURL(), Settings.Email);
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
            if (id.HasNoValue()) return NotFound("Invalid ID");

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

            if (error.HasValue())
            {
                return BadRequest(error);
            }

            return Ok(
                new
                {
                    model.FullName,
                    Token = Authentication
                            .GenerateToken(x => x.WithClaim(AccountModel.Claims.ID, model.AccountID)
                                                 .WithClaim(AccountModel.Claims.Email,model.UserName)
                                                 .WithClaim(Claims.Role, Roles.Owner))
                });
        }

        [NeedPermission(AccountsView.Perms.View)]
        [HttpGet("/api/view/accounts")]
        public ActionResult<AccountsView> ViewAccounts()
        {
            var view = new AccountsView();
            view.Load();
            return Ok(view);
        }
    }
}
