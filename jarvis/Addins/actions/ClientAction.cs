using Ninject;
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
