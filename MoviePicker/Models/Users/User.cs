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

        //TODO: hashing and stuff
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}