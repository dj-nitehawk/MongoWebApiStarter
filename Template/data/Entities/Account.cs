using MongoDB.Entities.Common;

namespace MongoWebApiStarter.Data.Entities
{
    public class Account : Entity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Address Address { get; set; }

        public bool IsEmailVerified { get; set; }
        public string EmailVerificationCode { get; set; }

    }
}