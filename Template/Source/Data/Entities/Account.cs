
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace Data
{
    public class Account : Entity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string Mobile { get; set; }

        [Preserve]
        public bool IsEmailVerified { get; set; }
        [Preserve]
        public string EmailVerificationCode { get; set; }
    }
}