using System;

namespace jarvis.client.common.ServiceClients
{
    public abstract class ServiceBase
    {
        protected readonly IJarvisRestClient _jarvisRestClient;
        protected readonly IConfiguration _configuration;
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