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
using jarvis.common.domain;
using jarvis.common.dtos.Requests;
using log4net;
using System.Linq;

namespace jarvis.client.common.ServiceClients
{
    public interface IJarvisRestClient
    {
        string BaseUrl { get; set; }
        T Execute<T>(Request restRequest) where T : new();
        void Execute(Request restRequest);
        void CheckForException(ResponseStatus responseStatus);
    }

    public class JarvisRestClient : IJarvisRestClient
    {
        private readonly ILog _log;
        private JsonServiceClient _jsonServiceClient;

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

        public Response Execute<Response>(Request restRequest) where Response : new()
        {
            _log.InfoFormat("Send request {0} to {1}", restRequest.GetType().Name, BaseUrl);
            var response = _jsonServiceClient.Send<Response>(restRequest);

            return response;
        }

        public void Execute(Request restRequest)
        {
            _log.InfoFormat("Send request {0} to {1}", restRequest.GetType().Name, BaseUrl);
            _jsonServiceClient.Send<object>(restRequest);
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