using FastEndpoints;
using FluentAssertions;
using MongoDB.Entities;
using System.Net.Http.Headers;
using Xunit;
using Xunit.Priority;
using Login = Account.Login;
using Save = Account.Save;
using Verify = Account.Verify;

namespace Tests;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class AccountTests : TestBase
{
    public AccountTests(AppFixture fixture) : base(fixture) { }

    private static readonly string EmailAddress = $"{Guid.NewGuid()}@email.com";
    private static string? UserID;
    private static string? JwtToken;

    [Fact, Priority(1)]
    public async Task Account_Creation()
    {
        var (rsp, res) = await App.AccountClient.POSTAsync<Save.Endpoint, Save.Request, Save.Response>(new()
        {
            City = "City",
            CountryCode = "LKA",
            EmailAddress = EmailAddress,
            FirstName = "Firstname",
            LastName = "Surname",
            Mobile = "0773469292",
            Password = "qqqqq123Q",
            State = "State",
            Street = "Street",
            Title = "Mr.",
            ZipCode = "10100",
        });

        rsp!.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        res!.EmailSent.Should().BeTrue();
        res.ID.Should().NotBeNullOrEmpty();

        UserID = res.ID;

        var acc = await DB.Find<Dom.Account>().OneAsync(UserID);

        acc!.Email.Should().Be(EmailAddress);
        acc.IsEmailVerified.Should().BeFalse();
    }

    [Fact, Priority(2)]
    public async Task Account_Verification()
    {
        var code = (await DB
            .Find<Dom.Account>()
            .OneAsync(UserID))!
            .EmailVerificationCode;

        await App.AccountClient.POSTAsync<Verify.Endpoint, Verify.Request>(new()
        {
            ID = UserID!,
            Code = code
        });

        var verified = await DB
            .Find<Dom.Account, bool>()
            .Match(a => a.ID == UserID)
            .Project(a => a.IsEmailVerified)
            .ExecuteSingleAsync();

        verified.Should().Be(true);
    }

    [Fact, Priority(3)]
    public async Task Account_Login()
    {
        var (rsp, res) = await App.AccountClient.POSTAsync<Login.Endpoint, Login.Request, Login.Response>(new()
        {
            UserName = EmailAddress,
            Password = "qqqqq123Q"
        });

        var acc = await DB.Find<Dom.Account>().OneAsync(UserID);

        rsp!.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        res!.FullName.Should().Be($"{acc!.Title} {acc.FirstName} {acc.LastName}");
        res.PermissionSet.Should().HaveCount(7);
        res.Token.Value.Should().NotBeNull();

        JwtToken = res.Token.Value;
    }

    [Fact, Priority(4)]
    public async Task Account_Retrieve()
    {
        App.AccountClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);

        var (_, res) = await App.AccountClient.GETAsync<Account.Get.Endpoint, Account.Get.Response>();

        var acc = await DB.Find<Dom.Account>().OneAsync(res!.AccountID);

        var match =
            acc!.Email == res.EmailAddress &&
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