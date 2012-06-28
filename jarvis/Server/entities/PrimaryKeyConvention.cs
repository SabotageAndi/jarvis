using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace jarvis.server.entities
{
    public class MsSqlPrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
           
            instance.GeneratedBy.Native();
        }

        
    }

    public class PostgreSqlPrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.CustomSqlType("serial");
        }
    }
}