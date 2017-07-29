using System;
using System.Configuration;
using Dao.Joblanes.Mapping.UserMap;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Model.JobLanes.Extensions;
using Model.JobLanes.Helper;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

namespace Services.Joblanes.Helper
{
    public static class NhSessionFactory
    {
       
        private static ISessionFactory _sessionFactory;
        private static FluentConfiguration cfg;
        private static Configuration config;
        public static ISessionFactory GetSessionFactory()
        {
            DbConnectionString.MssqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return _sessionFactory ?? (_sessionFactory = Fluently.Configure().
                        Database(MsSqlConfiguration.MsSql2012.ConnectionString(DbConnectionString.MssqlConnectionString)
                           .ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserProfileMap>())
                //.ExposeConfiguration(BuildSchema)
                .BuildSessionFactory());
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration obj)
        {
            var se = new SchemaExport(obj);
            se.Execute(true, true, false);
        }

        public static ISession OpenSession()
        {
            return GetSessionFactory().OpenSession();
        }

        public static String GetTableName(Type entityType)
        {
            if (config == null)
            {
                config = cfg.BuildConfiguration();
            }
            return config.GetClassMapping(entityType).RootTable.Name;
        }
    }
}