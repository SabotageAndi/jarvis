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

        protected JsonSerializer JsonSerializer
        {
            get { return new JsonSerializer(JsonParser.GetJsonSerializer()); }
        }

        protected Newtonsoft.Json.JsonSerializer JsonDeserializer
        {
            get { return JsonParser.GetJsonSerializer(); }
        }
    }
}
