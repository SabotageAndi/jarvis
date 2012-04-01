using Newtonsoft.Json;
using RestSharp;
using JsonSerializer = RestSharp.Serializers.JsonSerializer;

namespace EventHandler
{
    public class RestRequestFactory
    {
        private static JsonSerializer _jsonSerializer = new JsonSerializer(Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings()
                                                                                                                     {
                                                                                                                         MissingMemberHandling = MissingMemberHandling.Ignore,
                                                                                                                         NullValueHandling = NullValueHandling.Include,
                                                                                                                         DefaultValueHandling = DefaultValueHandling.Include,
                                                                                                                         DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                                                                                                                     }));

        public static RestRequest Create(string resource, Method method)
        {
            var restRequest = new RestRequest(resource, method);
            restRequest.JsonSerializer = _jsonSerializer;
            restRequest.RequestFormat = DataFormat.Json;

            return restRequest;
        }

    }
}