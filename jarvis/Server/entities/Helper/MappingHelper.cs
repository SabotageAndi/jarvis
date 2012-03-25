using FluentNHibernate.Mapping;

namespace jarvis.server.entities.Helper
{
    public class MappingHelper
    {
        public static void MapId<T>(ClassMap<T> map) where T : Entity
        {
            map.Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
        }
    }
}
