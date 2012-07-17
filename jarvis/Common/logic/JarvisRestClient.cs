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


using System;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceInterface.ServiceModel;
using jarvis.common.dtos.Requests;
using log4net;
using System.Linq;

namespace jarvis.common.logic
{
    public interface IJarvisRestClient
    {
        string BaseUrl { get; set; }
        T Execute<T>(Request restRequest, string httpMethod) where T : new();
        void Execute(Request restRequest, string httpMethod);
        void CheckForException(ResponseStatus responseStatus);
    }

    public class JarvisRestClient : IJarvisRestClient
    {
        private readonly ILog _log;
        private readonly JsonServiceClient _jsonServiceClient;

        public JarvisRestClient(ILog log)
        {
            _log = log;
            _jsonServiceClient = new JsonServiceClient();
        }

        public string BaseUrl
        {
            get { return _jsonServiceClient.BaseUri; }
            set { _jsonServiceClient.SetBaseUri(value); }
        }

        public Response Execute<Response>(Request restRequest, string httpMethod) where Response : new()
        {
            restRequest.Version = "1.0";

            _log.InfoFormat("Send request {0} to {1} via {2}", restRequest.GetType().Name, BaseUrl, httpMethod);
            _jsonServiceClient.HttpMethod = httpMethod;
            _jsonServiceClient.DisableAutoCompression = true;

            var response = _jsonServiceClient.Send<Response>(restRequest);
            _log.InfoFormat("Received request {0} to {1}", restRequest.GetType().Name, BaseUrl);

            return response;
        }

        public void ExecuteAsync<Response>(Request restRequest, Action<Response> onSuccess, Action<Response, Exception> onError, string httpMethod)
        {
            restRequest.Version = "1.0";
            _log.InfoFormat("Send request {0} to {1}", restRequest.GetType().Name, BaseUrl);
            _jsonServiceClient.HttpMethod = httpMethod; 
            _jsonServiceClient.SendAsync(restRequest,onSuccess, onError);
        }

        public void Execute(Request restRequest, string httpMethod)
        {
            restRequest.Version = "1.0";
            _log.InfoFormat("Send request {0} to {1}", restRequest.GetType().Name, BaseUrl);
            _jsonServiceClient.HttpMethod = httpMethod; 
            _jsonServiceClient.Send<object>(restRequest);
            _log.InfoFormat("Received request {0} to {1}", restRequest.GetType().Name, BaseUrl);
        }

        public void ExecuteAsync(Request restRequest, Action<object> onSuccess, Action<object, Exception> onError, string httpMethod)
        {
            restRequest.Version = "1.0";
            _log.InfoFormat("Send request {0} to {1}", restRequest.GetType().Name, BaseUrl);
            _jsonServiceClient.HttpMethod = httpMethod; 
            _jsonServiceClient.SendAsync(restRequest,onSuccess, onError);
        }

        public void CheckForException(ResponseStatus responseStatus)
        {
            if (responseStatus == null)
                return;

            if (!responseStatus.Errors.Any())
                return;

            throw new Exception(responseStatus.Message + Environment.NewLine + responseStatus.StackTrace);
        }
    }
}