using System.Text.Json;
using Native = System.Text.Json.JsonSerializer;

namespace EasyChallenges.Services
{
    public static class JsonDeserializer
    {
        private static readonly JsonSerializerOptions Options;

        static JsonDeserializer()
        {
            Options = new()
            {
                AllowTrailingCommas = true,
                IncludeFields = true,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = true
            };
            Options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        }


        public static T Deserialize<T>(string json) => Native.Deserialize<T>(json, Options);
    }
}
