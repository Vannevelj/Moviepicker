using System;
using Newtonsoft.Json;

namespace Models.Users
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("last_updated_on")]
        public DateTime LastUpdatedOn { get; set; }

        [JsonProperty("registered_on")]
        public DateTime RegisteredOn { get; set; }
    }
}