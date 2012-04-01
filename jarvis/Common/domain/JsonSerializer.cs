using Newtonsoft.Json;
using RestSharp.Deserializers;
using JsonSerializer = RestSharp.Serializers.JsonSerializer;

namespace jarvis.common.domain
{
    public class JsonParser
    {
        public static JsonSerializer Serializer = new JsonSerializer(Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings()
                                                                                                                    {
                                                                                                                        MissingMemberHandling = MissingMemberHandling.Ignore,
                                                                                                                        NullValueHandling = NullValueHandling.Include,
                                                                                                                        DefaultValueHandling = DefaultValueHandling.Include,
                                                                                                                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                                                                                                                    }));

        public static JsonDeserializer Deserializer = new JsonDeserializer();
    }
}