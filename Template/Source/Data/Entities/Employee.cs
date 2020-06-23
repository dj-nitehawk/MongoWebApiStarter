using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using MongoDB.Entities.Core;
using System;

namespace Data
{
    public class Employee : Entity
    {
        public bool IsDoctor { get; set; }
        public bool IsNurse { get; set; }
        public string Title { get; set; }
        public bool IsMale { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public One<Image> ProfilePicture { get; set; }
        public string Designation { get; set; }
        public string Description { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string[] RoleIDs { get; set; }

        [Preserve]
        public string SignUpCode { get; set; }
        [Preserve]
        public DateTime SignUpCodeExpiry { get; set; }
        [Preserve]
        public string UserName { get; set; }
        [Preserve]
        public string PasswordHash { get; set; }
        [Preserve]
        public One<VirtualPractice> VP { get; set; }
    }
}
