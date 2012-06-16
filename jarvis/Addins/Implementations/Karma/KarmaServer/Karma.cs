using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using jarvis.server.entities;

namespace jarvis.addins.karma.server
{
    public class Karma : Entity
    {
        public virtual string KarmaKey { get; set; }
        public virtual int KarmaValue { get; set; }
    }

    public class KarmaMap : ClassMap<Karma>
    {
        public KarmaMap()
        {
            Id(x => x.Id);
            Map(x => x.KarmaKey);
            Map(x => x.KarmaValue);
        }
    }
}
