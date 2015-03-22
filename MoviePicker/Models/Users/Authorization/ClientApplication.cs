namespace Models.Users.Authorization
{
    public class ClientApplication
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        ///     Time in minutes
        /// </summary>
        public int RefreshTokenLifeTime { get; set; }

        public string AllowedOrigin { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}