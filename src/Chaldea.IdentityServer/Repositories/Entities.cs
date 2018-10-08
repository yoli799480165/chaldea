using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chaldea.IdentityServer.Repositories
{
    public class Roles
    {
        public const string User = "user";
        public const string Admin = "admin";
    }

    public class User : IEntity<string>
    {
        public string Role { get; set; }

        public bool IsActive { get; set; }

        public string Address { get; set; }

        public string WebSite { get; set; }

        public bool EmailVerified { get; set; }

        public string Email { get; set; }

        public string FamilyName { get; set; }

        public string GivenName { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}