using Get = Account.Get;
using Login = Account.Login;
using Save = Account.Save;
using Verify = Account.Verify;

namespace Tests.Account;

public class AccountTests : TestClass<Fixture>
{
    public AccountTests(Fixture f, ITestOutputHelper o) : base(f, o) { }

    [Fact, Priority(1)]
    public async Task Account_Creation()
    {
        var req = Fixture.SaveRequest;

        var (rsp, res) = await Fixture.Client.POSTAsync<Save.Endpoint, Save.Request, Save.Response>(req);

        rsp!.IsSuccessStatusCode.Should().BeTrue();
        res!.EmailSent.Should().BeTrue();
        res.ID.Should().NotBeNullOrEmpty();

        Fixture.AccountID = res.ID;

        var acc = await DB.Find<Dom.Account>().OneAsync(res.ID);

        acc!.Email.Should().Be(req.EmailAddress.ToLower());
        acc.IsEmailVerified.Should().BeFalse();
    }

    [Fact, Priority(2)]
    public async Task Account_Verification()
    {
        var accountID = Fixture.AccountID;

        var code = (await DB
            .Find<Dom.Account>()
            .OneAsync(accountID))!
            .EmailVerificationCode;

        await Fixture.Client.POSTAsync<Verify.Endpoint, Verify.Request>(new()
        {
            ID = accountID!,
            Code = code
        });

        var verified = await DB
            .Find<Dom.Account, bool>()
            .Match(a => a.ID == accountID)
            .Project(a => a.IsEmailVerified)
            .ExecuteSingleAsync();

        verified.Should().BeTrue();
    }

    [Fact, Priority(3)]
    public async Task Account_Login()
    {
        var (rsp, res) = await Fixture.Client.POSTAsync<Login.Endpoint, Login.Request, Login.Response>(new()
        {
            UserName = Fixture.SaveRequest.EmailAddress,
            Password = Fixture.SaveRequest.Password
        });

        var acc = await DB.Find<Dom.Account>().OneAsync(Fixture.AccountID);

        rsp!.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        res!.FullName.Should().Be($"{acc!.Title} {acc.FirstName} {acc.LastName}");
        res.PermissionSet.Should().HaveCount(7);
        res.Token.Value.Should().NotBeNull();

        Fixture.JwtToken = res.Token.Value;
    }

    [Fact, Priority(4)]
    public async Task Account_Retrieve()
    {
        Fixture.Client.DefaultRequestHeaders.Authorization = new("Bearer", Fixture.JwtToken);

        var (_, res) = await Fixture.Client.GETAsync<Get.Endpoint, Get.Response>();

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