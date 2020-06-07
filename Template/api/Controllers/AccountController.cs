using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoWebApiStarter.Api.Auth;
using MongoWebApiStarter.Api.Base;
using MongoWebApiStarter.Api.Extensions;
using MongoWebApiStarter.Biz.Models;
using MongoWebApiStarter.Biz.Views;
using System.Security.Claims;

namespace MongoWebApiStarter.Api.Controllers
{
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("api/account")]
        public ActionResult Save(AccountModel model)
        {
            model.Save();
            model.SendVerificationEmail(BaseURL, Settings.Email);
            return Ok(new
            {
                model.ID,
                EmailSent = model.NeedsEmailVerification
            });
        }

        [Permission(AccountModel.Perms.Write)]
        [HttpPatch("api/account")]
        public ActionResult Update(AccountModel model)
        {
            model.ID = User.FindFirstValue(AccountModel.Claims.ID); //ignore client submitted id and read from token to prevent post tampering

            return Save(model);
        }

        [Permission(AccountModel.Perms.Read)]
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
            model.SingIn();

            if (model.HasErrors())
                return BadRequest(model.Errors());

            return Ok(
                new
                {
                    model.FullName,
                    Token = Authentication.GenerateToken(model.Claims)
                });
        }

        [Permission(AccountsView.Perms.Read)]
        [HttpGet("/api/accounts")]
        public AccountsView ViewAccounts()
        {
            var view = new AccountsView();
            view.Load();
            return view;
        }
    }
}
