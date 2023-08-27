using FastEndpoints;
using FluentAssertions;
using MongoDB.Entities;
using System.Net.Http.Headers;
using Xunit;
using Login = Account.Login;
using Save = Account.Save;
using Verify = Account.Verify;

namespace Tests;

public class AccountTests : TestBase
{
    public AccountTests(AppFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Account_Creation()
    {
        var email = $"{Guid.NewGuid()}@email.com";
        var (rsp, res) = await CreateAccount(email);

        rsp?.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        res?.EmailSent.Should().BeTrue();
        res?.ID.Should().NotBeNullOrEmpty();

        var acc = await DB.Find<Dom.Account>().OneAsync(res?.ID);

        acc!.Email.Should().Be(email);
        acc.IsEmailVerified.Should().BeFalse();
    }

    private Task<TestResult<Save.Response>> CreateAccount(string email)
    {
        return App.AccountClient.POSTAsync<Save.Endpoint, Save.Request, Save.Response>(new()
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
    }

    [Fact]
    public async Task Account_Verification()
    {
        var email = $"{Guid.NewGuid()}@email.com";

        var (_, res) = await CreateAccount(email);

        var verified = await VerifyAccount(res!.ID);

        verified.Should().Be(true);
    }

    private async Task<bool> VerifyAccount(string accountID)
    {
        var code = (await DB
            .Find<Dom.Account>()
            .OneAsync(accountID))!
            .EmailVerificationCode;

        await App.AccountClient.POSTAsync<Verify.Endpoint, Verify.Request>(new()
        {
            ID = accountID,
            Code = code
        });

        return await DB
            .Find<Dom.Account, bool>()
            .Match(a => a.ID == accountID)
            .Project(a => a.IsEmailVerified)
            .ExecuteSingleAsync();
    }

    [Fact]
    public async Task Account_Login()
    {
        var email = $"{Guid.NewGuid()}@email.com";

        var (_, res) = await CreateAccount(email);
        await VerifyAccount(res!.ID);

        var (acRsp, acRes) = await LoginToAccount(email);

        var acc = await DB.Find<Dom.Account>().OneAsync(res.ID);

        acRsp?.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        acRes?.FullName.Should().Be($"{acc.Title} {acc.FirstName} {acc.LastName}");
    }

    private Task<TestResult<Login.Response>> LoginToAccount(string email)
    {
        return App.AccountClient.POSTAsync<Login.Endpoint, Login.Request, Login.Response>(new()
        {
            UserName = email,
            Password = "qqqqq123Q"
        });
    }

    [Fact]
    public async Task Account_Retrieve()
    {
        var email = $"{Guid.NewGuid()}@email.com";

        var (_, res) = await CreateAccount(email);
        await VerifyAccount(res!.ID);

        var (_, acRes) = await LoginToAccount(email);

        App.AccountClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", acRes!.Token.Value);

        var (_, gRes) = await App.AccountClient.GETAsync<Account.Get.Endpoint, Account.Get.Response>();

        var acc = await DB.Find<Dom.Account>().OneAsync(gRes!.AccountID);

        var match =
            acc!.Email == gRes.EmailAddress &&
            acc.Title == gRes.Title &&
            acc.FirstName == gRes.FirstName &&
            acc.LastName == gRes.LastName &&
            acc.Address.Street == gRes.Street &&
            acc.Address.City == gRes.City &&
            acc.Address.State == gRes.State &&
            acc.Address.ZipCode == gRes.ZipCode &&
            acc.Address.CountryCode == gRes.CountryCode &&
            acc.Mobile == gRes.Mobile;

        match.Should().BeTrue();
    }
}