using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Entities;
using MongoWebApiStarter;
using System.Net.Http.Headers;

namespace Test
{
    [TestClass]
    public class AccountTests
    {
        public static HttpClient AccountClient { get; } = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b =>
            {
                b.ConfigureServices(s =>
                {
                    var settings = new Settings();
                    settings.Database.Host = "localhost";
                    settings.Database.Port = 27017;
                    settings.Database.Name = "MongoWebApiStarter-TEST";
                    settings.FileBucket.Host = "localhost";
                    settings.FileBucket.Port = 27017;
                    settings.FileBucket.Name = "MongoWebApiStarter-FileBucket-TEST";
                    s.AddSingleton(settings);
                });
            })
            .CreateClient();

        private string accountID = string.Empty;
        private string jwtToken = string.Empty;

        [TestMethod]
        public async Task CreateAccount()
        {
            var email = $"{Guid.NewGuid()}@email.com";

            var (rsp, res) = await AccountClient.PostAsync<
                Account.Save.Endpoint,
                Account.Save.Request,
                Account.Save.Response>(new()
                {
                    City = "City",
                    CountryCode = "LKA",
                    EmailAddress = email,
                    FirstName = "Firstname",
                    LastName = "Surname",
                    Mobile = "0773469292",
                    Password = "qqqqq123Q",
                    State = "State",
                    Street = "Street",
                    Title = "Mr.",
                    ZipCode = "10100",
                });

            accountID = res.ID;

            rsp?.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            res?.EmailSent.Should().BeTrue();
            res?.ID.Should().NotBeNullOrEmpty();

            var acc = await DB.Find<Dom.Account>().OneAsync(res?.ID);

            acc.Email.Should().Be(email);
            acc.IsEmailVerified.Should().BeFalse();
        }

        [TestMethod]
        public async Task ValidateAccount()
        {
            await CreateAccount();

            var code = (await DB
                .Find<Dom.Account>().OneAsync(accountID))
                .EmailVerificationCode;

            await AccountClient.PostAsync<
                Account.Verify.Endpoint,
                Account.Verify.Request>(new()
                {
                    ID = accountID,
                    Code = code
                });

            var verified = await DB.Find<Dom.Account, bool>()
                                   .Match(a => a.ID == accountID)
                                   .Project(a => a.IsEmailVerified)
                                   .ExecuteSingleAsync();

            verified.Should().Be(true);
        }

        [TestMethod]
        public async Task AccountLogin()
        {
            await ValidateAccount();

            var acc = await DB.Find<Dom.Account>().OneAsync(accountID);

            var (rsp, res) = await AccountClient.PostAsync<
                Account.Login.Endpoint,
                Account.Login.Request,
                Account.Login.Response>(new()
                {
                    UserName = acc.Email,
                    Password = "qqqqq123Q"
                });

            jwtToken = res.Token.Value;

            rsp?.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            res?.FullName.Should().Be($"{acc.Title} {acc.FirstName} {acc.LastName}");
        }

        [TestMethod]
        public async Task GetAccount()
        {
            await AccountLogin();

            AccountClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", jwtToken);

            var res = await AccountClient.GetAsync<
                Account.Get.Endpoint,
                Account.Get.Response>();

            var acc = await DB.Find<Dom.Account>().OneAsync(accountID);

            var match =
                acc.Email == res?.EmailAddress &&
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
