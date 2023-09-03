using Save = Account.Save;

namespace Tests.Account;

public class Fixture : TestFixture<Program>
{
    public Fixture(IMessageSink s) : base(s) { }

    internal Save.Request SaveRequest { get; private set; } = default!;
    public string AccountID { get; set; } = "";
    public string JwtToken { get; set; } = "";

    protected override Task SetupAsync()
    {
        SaveRequest = Fake.SaveRequest();
        return Task.CompletedTask;
    }

    protected override Task TearDownAsync()
        => DB.DeleteAsync<Dom.Account>(a => a.ID == AccountID);
}