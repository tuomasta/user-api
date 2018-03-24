using System;

namespace DataAccess
{
    public class AuthentincationInfo
    {
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
    }
}
