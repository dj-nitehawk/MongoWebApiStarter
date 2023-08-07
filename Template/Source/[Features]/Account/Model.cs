using System.ComponentModel;

namespace Account;

public class Model
{
    [DefaultValue("email@domain.com")]
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string CountryCode { get; set; }
    public string Mobile { get; set; }
}