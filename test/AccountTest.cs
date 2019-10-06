using App.Api.Controllers;
using App.Biz.Models;
using App.Biz.Settings;
using App.Data.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Entities;
using System;
using System.Linq;
using System.Security.Claims;

namespace App.Test
{
    [TestClass]
    public class AccountTest
    {
        private AppSettings settings = new AppSettings();

        [TestMethod]
        public void saving_a_new_account()
        {
            var email = $"{Guid.NewGuid().ToString()}@email.com";
            var account = new AccountModel
            {
                EmailAddress = email,
                Password = "passwordABC123",
            };

            var validator = new AccountModel.Validator();
            validator.ShouldNotHaveValidationErrorFor(v => v.Password, account.Password);

            var cnt = new AccountController(settings);
            var res = cnt.Save(account);

            res.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public void fail_login_with_unverified_email()
        {
            var email = $"{Guid.NewGuid().ToString()}@email.com";
            var account = new AccountModel
            {
                EmailAddress = email,
                Password = "passwordABC123",
            };

            var cnt = new AccountController(settings);
            cnt.Save(account);

            var res = cnt.Login(new LoginModel
            {
                UserName = account.EmailAddress,
                Password = account.Password
            });

            res.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void login_with_username_and_password()
        {
            var email = $"{Guid.NewGuid().ToString()}@email.com";
            var account = new AccountModel
            {
                EmailAddress = email,
                Password = "passwordABC123",
            };

            var cnt = new AccountController(settings);
            cnt.Save(account);

            DB.Update<Account>()
              .Match(a => a.Email == email)
              .Modify(a => a.IsEmailVerified, true)
              .Execute();

            var res = cnt.Login(new LoginModel
            {
                UserName = account.EmailAddress,
                Password = account.Password
            });

            res.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public void existing_email_validation()
        {
            var email = $"{Guid.NewGuid().ToString()}@email.com";
            var account = new AccountModel
            {
                EmailAddress = email,
                Password = "passwordABC",
            };
            account.Save();

            var validator = new AccountModel.Validator();
            validator.ShouldHaveValidationErrorFor(v => v.EmailAddress, email);
        }

        [TestMethod]
        public void retrieving_account_by_id()
        {
            var email = $"{Guid.NewGuid().ToString()}@email.com";
            var account = new AccountModel
            {
                EmailAddress = email,
                Password = "password",
            };
            account.Save();

            var cnt = new AccountController(settings);
            account = cnt.Retrieve(account.ID).Value;
            account.EmailAddress.Should().Be(email);
        }

        [TestMethod]
        public void updating_account()
        {
            var email = $"{Guid.NewGuid().ToString()}@email.com";
            var account = new AccountModel
            {
                EmailAddress = email,
                Password = "password",
            };
            account.Save();

            var cnt = new AccountController(settings);
            cnt.SetClaims(new[] { new Claim(AccountModel.Claims.ID, account.ID) });
            account = cnt.Retrieve(account.ID).Value;
            account.Password = "updated password";
            cnt.Update(account).Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public void email_validation_needed_or_not()
        {
            var email = $"{Guid.NewGuid().ToString()}@email.com";
            var account = new AccountModel { EmailAddress = email };

            //brand new a/c - needs verification
            account.CheckIfEmailValidationIsNeeded();
            account.NeedsEmailVerification.Should().BeTrue();

            account.Password = "test";
            account.Save();

            //new email address - needs verification
            account.EmailAddress = $"{Guid.NewGuid().ToString()}@email.com";
            account.CheckIfEmailValidationIsNeeded();
            account.NeedsEmailVerification.Should().BeTrue();

            //same initial email - no need for verification
            account.EmailAddress = email;
            account.CheckIfEmailValidationIsNeeded();
            account.NeedsEmailVerification.Should().BeFalse();
        }

        [TestMethod]
        public void validation_email_gets_added_to_queue()
        {
            var guid = Guid.NewGuid().ToString();

            var account = new AccountModel
            {
                ID = ObjectId.GenerateNewId().ToString(),
                EmailAddress = guid
            };
            account.NeedsEmailVerification = true;

            account.SendVerificationEmail("http://localhost:8888/", settings.Email);

            var msg = DB.Find<EmailMessage>()
                        .Match(e => e.ToEmail == guid)
                        .Execute()
                        .Single();

            msg.BodyHTML.Should().Contain("http://localhost:8888/#/account/" + account.ID + "-");
        }

        [TestMethod]
        public void email_validation_controller()
        {
            var account = new Account { ID = ObjectId.GenerateNewId().ToString(), EmailVerificationCode = "123456" };
            account.Save();

            var cnt = new AccountController(settings);
            cnt.ValidateEmail(account.ID, account.EmailVerificationCode);

            account = DB.Find<Account>().One(account.ID);

            account.IsEmailVerified.Should().BeTrue();
        }
    }
}
