using System;
using System.Collections.Generic;
using Models.Movies;
using Newtonsoft.Json;

namespace Models.Utilities
{
    public class LanguageConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var languages = new List<Language>();
            while (reader.TokenType != JsonToken.EndArray && reader.Read())
            {
                var language = reader.Value as string;
                if (!string.IsNullOrWhiteSpace(language))
                {
                    languages.Add(new Language(language, string.Empty));
                }
            }
            return languages;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (ICollection<>);
        }
    }
}