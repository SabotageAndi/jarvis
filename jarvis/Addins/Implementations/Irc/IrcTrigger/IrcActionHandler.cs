using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jarvis.addins.actions;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.irctrigger
{
    public class IrcActionHandler : ActionHandler
    {
        public override string ActionGroup
        {
            get { return "Irc"; }
        }

        public override bool CanHandleAction(ActionDto actionDto)
        {
            return actionDto.ActionGroup == "Irc";
        }

        public override ActionResultDto DoAction(ActionDto actionDto)
        {
            switch (actionDto.Action)
            {
                case "SendMessage":
                    {
                        var channel = GetParameter(actionDto, "Irc", "Channel");
                        var message = GetParameter(actionDto, "Irc", "Message");
                        return SendMessage(channel.Value, message.Value);
                    }
            }

            throw new ArgumentOutOfRangeException();
        }

        private ActionResultDto SendMessage(string channelName, string message)
        {
            var channel = Irc.Client.Channels.Where(ch => ch.Name == channelName).SingleOrDefault();

            if (channel != null)
            {
                Irc.Client.LocalUser.SendMessage(channel, message);
            }

            return new ActionResultDto();
        }
    }
}
