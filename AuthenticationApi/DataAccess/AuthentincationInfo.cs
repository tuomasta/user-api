using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataAccess
{
    [BsonIgnoreExtraElements]
    public class AuthentincationInfo
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
    }
}
