// J.A.R.V.I.S. - Just A Rather Very Intelligent System
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using FluentNHibernate.Mapping;
using jarvis.common.domain;
using jarvis.server.entities.Helper;

namespace jarvis.server.entities.Management
{
    public class Client : Entity
    {
        public virtual string Name { get; set; }
        public virtual bool IsOnline { get; set; }
        public virtual string Hostname { get; set; }
        public virtual ClientTypeEnum Type { get; set; }
    }

    public class ClientMap : ClassMap<Client>
    {
        public ClientMap()
        {
            MappingHelper.MapId(this);
            Map(x => x.Name);
            Map(x => x.IsOnline);
            Map(x => x.Hostname);
            Map(x => x.Type);
        }
    }
}