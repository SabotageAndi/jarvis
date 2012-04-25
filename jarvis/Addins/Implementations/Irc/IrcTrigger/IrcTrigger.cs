using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using IrcDotNet;
using RestSharp.Serializers;
using jarvis.addins.trigger;
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Eventhandling.Parameter;

namespace jarvis.addins.irctrigger
{
    public class IrcTrigger : Trigger
    {
        private IrcClient _ircClient;
        public ITriggerService TriggerService { get; set; }

        public override void run()
        {
            _ircClient.Registered += _ircClient_Registered;

            _ircClient.Connect("irc.freenode.net", false, new IrcUserRegistrationInfo()
                                                              {
                                                                  NickName = "jarvis_bot",
                                                                  RealName = "J.A.R.V.I.S.",
                                                                  UserName = "jarvis_bot",
                                                                  Password = "aMpYScuHg7AC"
                                                              });

            _ircClient.Connected += _ircClient_Connected;
            _ircClient.Error += client_Error;
        }

        void _ircClient_Connected(object sender, EventArgs e)
        {
        }

    
        void _ircClient_Registered(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;

            client.Channels.Join("##jarvistest");

            client.LocalUser.JoinedChannel += LocalUser_JoinedChannel;
            client.LocalUser.LeftChannel += LocalUserOnLeftChannel;

            client.LocalUser.MessageReceived += OnMessageReceived;
        }

        void client_Error(object sender, IrcErrorEventArgs e)
        {
            throw new NotImplementedException();
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
            _ircClient.Quit();
            _ircClient.Disconnect();
        }

        public override void init()
        {
            _ircClient = new IrcClient();
            _ircClient.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);

        }
    }
}
