using Newtonsoft.Json;

namespace Models.Movies
{
    public class Keyword
    {
        public Keyword()
        {
        }

        public Keyword(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Keyword;
            if (other == null)
            {
                return false;
            }

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public virtual void Update(Keyword keyword)
        {
            Name = keyword.Name;
        }
    }
}