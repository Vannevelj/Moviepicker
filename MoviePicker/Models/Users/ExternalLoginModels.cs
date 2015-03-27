using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Models.Users
{
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string State { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string ExternalAccessToken { get; set; }
    }

    public class ParsedExternalAccessToken
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("app_id")]
        public string ApplicationId { get; set; }
    }
}