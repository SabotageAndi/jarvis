using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using jarvis.common.dtos;

namespace jarvis.client.trigger.common
{
    public class TriggerLogic
    {

        public TriggerLogic()
        {
        }

        public void trigger(EventDto eventDto)
        {
            var client = new RestClient("http://localhost:3125/TriggerService.svc");

            var request = new RestRequest("trigger", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(eventDto);

            client.Execute(request);
        }
    }
}
