using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;
using MongoWebApiStarter.Tests;
using ServiceStack;
using System;
using System.Linq;

namespace MongoWebApiStarter.Main_Account
{
    [TestClass]
    public class Account
    {
        private readonly JsonServiceClient client = new JsonServiceClient(Context.BaseURL);

        private string CreateAccount()
        {
            var savReq = new Main.Account.Save.Request
            {
                City = "City",
                CountryCode = "LKA",
                EmailAddress = $"{Guid.NewGuid()}@email.com",
                FirstName = "Firstname",
                LastName = "Surname",
                Mobile = "0773469292",
                Password = "qqqqq123Q",
                State = "State",
                Street = "Street",
                Title = "Mr.",
                ZipCode = "10100",
            };

            var res = client.Post(savReq);

            res.EmailSent.Should().BeTrue();
            res.ID.Should().NotBeNullOrEmpty();

            var acc = DB.Find<Dom.Account>().One(res.ID);

            acc.Email.Should().Be(savReq.EmailAddress);
            acc.IsEmailVerified.Should().BeFalse();

            return acc.ID;
        }

        [TestMethod]
        public void Create()
        {
            CreateAccount();
        }

        private void ValidateAccount(string id)
        {
            var code = DB.Find<Dom.Account>().One(id).EmailVerificationCode;

            var vReq = new Main.Account.Verify.Request
            {
                ID = id,
                Code = code
            };

            client.Get(vReq);

            var verified = DB.Find<Dom.Account, bool>()
                             .Match(a => a.ID == id)
                             .Project(a => a.IsEmailVerified)
                             .Execute()
                             .Single();

            verified.Should().Be(true);
        }

        [TestMethod]
        public void Validate()
        {
            var id = CreateAccount();
            ValidateAccount(id);
        }

        private string AccountLogin(string id)
        {
            var acc = DB.Find<Dom.Account>().One(id);

            var req = new Main.Account.Login.Request
            {
                UserName = acc.Email,
                Password = "qqqqq123Q"
            };

            var res = client.Post(req);

            res.FullName.Should().Be($"{acc.Title} {acc.FirstName} {acc.LastName}");

            return res.Token.Value;
        }

        [TestMethod]
        public void Login()
        {
            var id = CreateAccount();
            ValidateAccount(id);
            AccountLogin(id);
        }

        [TestMethod]
        public void Get()
        {
            var id = CreateAccount();
            ValidateAccount(id);
            var token = AccountLogin(id);

            var req = new Main.Account.Get.Request
            {
                ID = id
            };

            client.BearerToken = token;

            var res = client.Get(req);

            var acc = DB.Find<Dom.Account>().One(id);

            var match =
                acc.Email == res.EmailAddress &&
                acc.Title == res.Title &&
                acc.FirstName == res.FirstName &&
                acc.LastName == res.LastName &&
                acc.Address.Street == res.Street &&
                acc.Address.City == res.City &&
                acc.Address.State == res.State &&
                acc.Address.ZipCode == res.ZipCode &&
                acc.Address.CountryCode == res.CountryCode &&
                acc.Mobile == res.Mobile;

            match.Should().BeTrue();
        }
    }
}
