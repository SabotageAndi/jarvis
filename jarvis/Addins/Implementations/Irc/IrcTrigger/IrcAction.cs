using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.irctrigger
{
    public interface IIrcAction
    {
        void SendMessage(string hostname, string channel, string message);
    }

    public class IrcAction : actions.Action, IIrcAction
    {
        public override string PropertyName
        {
            get { return "Irc"; }
        }

       public void SendMessage(string hostname, string channel, string message)
       {
           var actionDto = new ActionDto();
           actionDto.ActionGroup = "Irc";
           actionDto.Action = "SendMessage";

           actionDto.Parameters.Add(new ParameterDto(){Category = "Irc", Name = "Client", Value = hostname});
           actionDto.Parameters.Add(new ParameterDto(){Category = "Irc", Name = "Channel", Value = channel});
           actionDto.Parameters.Add(new ParameterDto(){Category = "Irc", Name = "Message", Value = message});

           ActionService.Execute(actionDto);
       }
    }
}
