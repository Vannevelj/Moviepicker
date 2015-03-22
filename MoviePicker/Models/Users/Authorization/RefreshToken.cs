using System;

namespace Models.Users.Authorization
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string ProtectedTicket { get; set; }
        public string ClientApplicationId { get; set; }
        public virtual ClientApplication ClientApplication { get; set; }
    }
}