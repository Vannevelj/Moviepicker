using Newtonsoft.Json;

namespace Models.Movies
{
    public class Language
    {
        public Language()
        {
        }

        public Language(string iso, string name)
        {
            Iso = iso;
            Name = name;
        }

        [JsonProperty("iso_639_1")]
        public string Iso { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Language))
            {
                return false;
            }

            return ((Language) obj).Iso == Iso;
        }

        public override int GetHashCode()
        {
            return Iso.GetHashCode();
        }

        public virtual void Update(Language language)
        {
            Name = language.Name;
        }
    }
}