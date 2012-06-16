using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using IrcDotNet;
using Ninject;
using RestSharp.Serializers;
using jarvis.addins.trigger;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Eventhandling.Parameter;

namespace jarvis.addins.irctrigger
{
    public class IrcTrigger : Trigger
    {
        [Inject]
        public IConfiguration Configuration { get; set; }

       
        [Inject]
        public ITriggerService TriggerService { get; set; }

        private IrcTriggerConfigurationSection ConfigurationSection
        {
            get { return Configuration.Configuration.GetSection("IrcTrigger") as IrcTriggerConfigurationSection; }
        }

        public override void run()
        {
            Irc.Client.Registered += Irc_Client_Registered;

            Irc.Client.Connected += Irc_Client_Connected;
            Irc.Client.Error += client_Error;

            foreach (var configElement in GetConfigElements)
            {
                Irc.Client.Connect(configElement.Network, false, new IrcUserRegistrationInfo()
                                                                  {
                                                                      NickName = configElement.Nickname, 
                                                                      RealName = configElement.Realname,
                                                                      UserName = configElement.Username, 
                                                                      Password = configElement.Password 
                                                                  });
            }

        }

        private List<IrcTriggerConfigurationElement> GetConfigElements
        {
            get { return ConfigurationSection.ConfigElementCollection.Cast<IrcTriggerConfigurationElement>().ToList(); }
        }

        void Irc_Client_Connected(object sender, EventArgs e)
        {
            Debug.Assert(true);
        }

    
        void Irc_Client_Registered(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;

            var ircNetworkConfigElement = GetConfigElements.FirstOrDefault();

            if (ircNetworkConfigElement == null)
                return;

            


            client.LocalUser.JoinedChannel += LocalUser_JoinedChannel;
            client.LocalUser.LeftChannel += LocalUserOnLeftChannel;

            client.LocalUser.MessageReceived += OnMessageReceived;

            var channels = ircNetworkConfigElement.Channels.Split(',');

            foreach (var channel in channels)
            {
                client.Channels.Join(channel);
            }
        }

        void client_Error(object sender, IrcErrorEventArgs e)
        {
        }

        private void LocalUserOnLeftChannel(object sender, IrcChannelEventArgs ircChannelEventArgs)
        {
            ircChannelEventArgs.Channel.PreviewMessageReceived -= OnMessageReceived;
        }

        void LocalUser_JoinedChannel(object sender, IrcChannelEventArgs e)
        {
            e.Channel.MessageReceived += OnMessageReceived;
        }


        void OnMessageReceived(object sender, IrcMessageEventArgs e)
        {
            var jsonSerializer = new JsonSerializer(JsonParser.GetJsonSerializer());

            TriggerService.EventHappend(new EventDto()
            {
                EventGroupTypes = EventGroupTypes.Irc,
                EventType = EventType.Changed,
                TriggeredDate = DateTime.UtcNow,
                Data = jsonSerializer.Serialize(new IrcEventParameter()
                                                    {
                                                        User = e.Source.Name,
                                                        Message = e.Text,
                                                        Channels = String.Join(",", e.Targets.Select(t => t.Name))
                                                    })
            });
        }

        public override void deinit()
        {
            Irc.Client.Quit();
            Irc.Client.Disconnect();
        }

        public override void init()
        {
        }
    }
}
