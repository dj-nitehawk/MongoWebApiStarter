namespace Dom;

sealed class Account : Entity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Address Address { get; set; }
    public string Mobile { get; set; }

    [Preserve] public bool IsEmailVerified { get; set; }
    [Preserve] public string EmailVerificationCode { get; set; }

    static Account()
    {
        DB.Index<Account>()
          .Key(a => a.Email, KeyType.Ascending)
          .Option(o => o.Unique = true)
          .CreateAsync();

        DB.Index<Account>()
          .Key(a => a.EmailVerificationCode, KeyType.Ascending)
          .CreateAsync();
    }
}