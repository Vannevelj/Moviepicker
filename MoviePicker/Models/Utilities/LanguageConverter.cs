using System;
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
            string language = null;
            try
            {
                language = reader.Value.ToString();
            }
            catch
            {
                // Log
            }

            return new Language
            {
                Iso = language
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (string);
        }
    }
}