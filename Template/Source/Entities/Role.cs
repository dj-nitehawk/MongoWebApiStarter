namespace Dom;

internal sealed class Role : Entity
{
    public string Name { get; set; }
    public bool SystemRole { get; set; }
    public IEnumerable<string> Permissions { get; set; }
    public One<Account> Account { get; set; }

    static Role()
    {
        DB.Index<Role>()
          .Key(r => r.Account.ID, KeyType.Ascending)
          .CreateAsync();

        DB.Index<Role>()
          .Key(r => r.SystemRole, KeyType.Ascending)
          .CreateAsync();
    }
}