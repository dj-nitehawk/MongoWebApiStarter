namespace Main.Account
{
    public class Model
    {
        public string ID { get; set; }
        public string VPName { get; set; }
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
        public decimal TimeZoneOffsetHrs { get; set; } // example: 5.5 or -5.5
        public string Mobile { get; set; }
    }
}
