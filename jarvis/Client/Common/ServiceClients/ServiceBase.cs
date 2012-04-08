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

namespace jarvis.client.common.ServiceClients
{
    public abstract class ServiceBase
    {
        protected readonly IConfiguration _configuration;
        private readonly IJarvisRestClient _jarvisRestClient;
        private bool _clientInitialized = false;

        protected ServiceBase(IJarvisRestClient jarvisRestClient, IConfiguration configuration)
        {
            _jarvisRestClient = jarvisRestClient;
            _configuration = configuration;
        }

        protected IJarvisRestClient JarvisRestClient
        {
            get
            {
                if (!_clientInitialized)
                {
                    _jarvisRestClient.BaseUrl = String.Format("{0}/{1}/", _configuration.ServerUrl, ServiceName);

                    _clientInitialized = true;
                }
                return _jarvisRestClient;
            }
        }

        protected abstract string ServiceName { get; }
    }
}