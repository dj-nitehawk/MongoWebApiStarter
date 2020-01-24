namespace MongoWebApiStarter.Biz.Models
{
    public partial class AccountModel
    {
        public bool NeedsEmailVerification = false;

        public string ID { get; set; }

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

        public bool IsEmailVerified { get; set; }
    }
}
