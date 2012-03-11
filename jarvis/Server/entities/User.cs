using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace jarvis.server.entities
{
    public class User : Entity
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
            Map(x => x.Username);
            Map(x => x.Password);
        }
    }
}
