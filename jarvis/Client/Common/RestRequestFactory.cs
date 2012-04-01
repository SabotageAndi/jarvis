using Newtonsoft.Json;
using RestSharp;
using jarvis.common.domain;

namespace jarvis.client.common
{
    public class RestRequestFactory
    {
        public static RestRequest Create(string resource, Method method)
        {
            var restRequest = new RestRequest(resource, method);
            restRequest.JsonSerializer = JsonParser.Serializer;
            restRequest.RequestFormat = DataFormat.Json;
            return restRequest;
        }

    }
}