using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using jarvis.client;
using jarvis.client.common;

namespace linux
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting up Client ...");
            
            Bootstrapper.Init<ActionTriggerClient>("linux-client");

            var client = Bootstrapper.Container.Get<Client>();

            client.Init(Bootstrapper.Container);
            client.Run();
            Console.WriteLine("Client running ...");

            Console.ReadLine();
        }
    }

}
