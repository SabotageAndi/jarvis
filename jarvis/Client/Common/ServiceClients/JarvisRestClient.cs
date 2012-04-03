using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using jarvis.common.domain;

namespace jarvis.client.common.ServiceClients
{
    public interface IJarvisRestClient
    {
        RestRequest CreateRequest(string resource, Method method);
        T Execute<T>(RestRequest restRequest) where T : new();
        void Execute(RestRequest restRequest);
        string BaseUrl { get; set; }
    }

    public class JarvisRestClient : IJarvisRestClient
    {
        private readonly RestClient _restClient;

        public JarvisRestClient()
        {
            _restClient = new RestClient();
            _restClient.FollowRedirects = true;
        }
        
        public RestRequest CreateRequest(string resource, Method method)
        {
            var restRequest = new RestRequest(resource, method);
            restRequest.JsonSerializer = JsonParser.Serializer;
            restRequest.RequestFormat = DataFormat.Json;
            return restRequest;
        }

        public string BaseUrl { get { return _restClient.BaseUrl; } set { _restClient.BaseUrl = value; } }

        public void Execute(RestRequest restRequest)
        {
            var restResponse = _restClient.Execute(restRequest);
            
            if(restResponse.ErrorException != null)
            {
                throw restResponse.ErrorException;
            }
        }

        public T Execute<T>(RestRequest restRequest) where T : new()
        {
            var restResponse = _restClient.Execute<T>(restRequest);

            if(restResponse.ErrorException != null)
            {
                throw restResponse.ErrorException;
            }

            return restResponse.Data;
        }
    }
}
