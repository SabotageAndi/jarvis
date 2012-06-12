using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrcDotNet;

namespace jarvis.addins.irctrigger
{
    internal class Irc
    {
        static Irc()
        {
            Client = new IrcClient();
            Client.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
        }

        public static IrcClient Client { get; set; }
    }
}
