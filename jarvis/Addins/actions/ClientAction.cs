using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using jarvis.common.domain;

namespace jarvis.addins.actions
{
    public abstract class ClientAction
    {
        [Inject]
        public client.common.ServiceClients.IActionService ActionService { get; set; }

        public abstract string PropertyName { get; }

        protected JarvisJsonSerializer JsonSerializer
        {
            get { return JsonParser.GetJsonSerializer(); }
        }

        protected JarvisJsonSerializer JsonDeserializer
        {
            get { return JsonParser.GetJsonSerializer(); }
        }
    }
}
