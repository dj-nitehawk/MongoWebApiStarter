using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;
using ServiceStack;
using System;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Tests
{
    [TestClass]
    public class Account_Tests
    {
        private readonly JsonServiceClient client = new JsonServiceClient(Context.BaseURL);

        private async Task<string> CreateAccount()
        {
            var savReq = new Account.Save.Request
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

            var res = await client.PostAsync(savReq);

            res.EmailSent.Should().BeTrue();
            res.ID.Should().NotBeNullOrEmpty();

            var acc = await DB.Find<Dom.Account>().OneAsync(res.ID);

            acc.Email.Should().Be(savReq.EmailAddress);
            acc.IsEmailVerified.Should().BeFalse();

            return acc.ID;
        }

        [TestMethod]
        public Task Create()
        {
            return CreateAccount();
        }

        private async Task ValidateAccount(string id)
        {
            var code = (await DB.Find<Dom.Account>().OneAsync(id))
                       .EmailVerificationCode;

            var vReq = new Account.Verify.Request
            {
                ID = id,
                Code = code
            };

            await client.GetAsync(vReq);

            var verified = await DB.Find<Dom.Account, bool>()
                                   .Match(a => a.ID == id)
                                   .Project(a => a.IsEmailVerified)
                                   .ExecuteSingleAsync();

            verified.Should().Be(true);
        }

        [TestMethod]
        public async Task Validate()
        {
            var id = await CreateAccount();
            await ValidateAccount(id);
        }

        private async Task<string> AccountLogin(string id)
        {
            var acc = await DB.Find<Dom.Account>().OneAsync(id);

            var req = new Account.Login.Request
            {
                UserName = acc.Email,
                Password = "qqqqq123Q"
            };

            var res = await client.PostAsync(req);

            res.FullName.Should().Be($"{acc.Title} {acc.FirstName} {acc.LastName}");

            return res.Token.Value;
        }

        [TestMethod]
        public async Task Login()
        {
            var id = await CreateAccount();
            await ValidateAccount(id);
            await AccountLogin(id);
        }

        [TestMethod]
        public async Task Get()
        {
            var id = await CreateAccount();
            await ValidateAccount(id);
            var token = await AccountLogin(id);

            var req = new Account.Get.Request
            {
                ID = id
            };

            client.BearerToken = token;

            var res = await client.GetAsync(req);

            var acc = await DB.Find<Dom.Account>().OneAsync(id);

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
