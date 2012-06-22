// J.A.R.V.I.S. - Just A Rather Very Intelligent System
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using RestSharp;
using RestSharp.Serializers;
using jarvis.common.domain;

namespace jarvis.client.common.ServiceClients
{
    public interface IJarvisRestClient
    {
        string BaseUrl { get; set; }
        RestRequest CreateRequest(string resource, Method method);
        T Execute<T>(RestRequest restRequest) where T : new();
        void Execute(RestRequest restRequest);
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
            restRequest.JsonSerializer = JsonParser.GetJsonSerializer();
            restRequest.RequestFormat = DataFormat.Json;
            return restRequest;
        }

        public string BaseUrl
        {
            get { return _restClient.BaseUrl; }
            set { _restClient.BaseUrl = value; }
        }

        public void Execute(RestRequest restRequest)
        {
            var restResponse = _restClient.Execute(restRequest);

            if (restResponse.ErrorException != null)
            {
                throw restResponse.ErrorException;
            }
        }

        public T Execute<T>(RestRequest restRequest) where T : new()
        {
            var restResponse = _restClient.Execute<T>(restRequest);

            if (restResponse.ErrorException != null)
            {
                throw restResponse.ErrorException;
            }

            return restResponse.Data;
        }
    }
}