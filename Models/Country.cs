using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace ApiDataCollection.Models
{
    public class CountryNameConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<string>));
        }

        public override string ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Object && token["official"] != null)
            {
                return token["official"].ToString();
            }
            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    class ArrayToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<string>));
        }

        public override string ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Null)
            {
                return null;
            }
            if (token.Type == JTokenType.Array)
            {
                return token.FirstOrDefault().ToString();
            }
            return token.ToString();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class Country
    {
        [JsonConverter(typeof(CountryNameConverter))]
        public string Name { get; set; }

        [JsonConverter(typeof(ArrayToStringConverter))]
        public string Capital { get; set; }

        public int Population { get; set; }
        // Add other properties as needed
    }
}
