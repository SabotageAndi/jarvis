using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jarvis.addins.actions;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.karma.client
{
    public interface IKarmaClientAction
    {
        int IncreaseKarma(string key);
        int DecreaseKarma(string key);
        string GetStats();
        string GetKarma();
    }

    public class KarmaClientAction : ClientAction, IKarmaClientAction
    {
        public override string PropertyName
        {
            get { return "Karma"; }
        }

        public int IncreaseKarma(string key)
        {
            return SendKarmaKeyAction("Increase", key);
        }

        public string GetStats()
        {
            var actionDto = new ActionDto();
            actionDto.ActionGroup = "Karma";
            actionDto.Action = "Stats";

            var result = ActionService.Execute(actionDto);

            return result.Data;
        }

        public string GetKarma()
        {
            var actionDto = new ActionDto();
            actionDto.ActionGroup = "Karma";
            actionDto.Action = "GetKarma";

            var result = ActionService.Execute(actionDto);

            return result.Data;
        }

        private int SendKarmaKeyAction(string action, string key)
        {
            var actionDto = new ActionDto();
            actionDto.ActionGroup = "Karma";
            actionDto.Action = action;

            actionDto.Parameters.Add(new ParameterDto() {Category = "Karma", Name = "Key", Value = key});

            var result = ActionService.Execute(actionDto);

            return Convert.ToInt32(result.Data);
        }


        public int DecreaseKarma(string key)
        {
            return SendKarmaKeyAction("Decrease", key);
        }
    }
}
