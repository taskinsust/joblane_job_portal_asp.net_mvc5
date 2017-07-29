using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    class CompanyMap : BaseClassMap<Company, long>
    {
        public CompanyMap()
        {
            Table("Company");
            LazyLoad();

            Map(x => x.Name).Column("Name");
            Map(x => x.ContactPerson).Column("ContactPerson");
            Map(x => x.ImageGuid).Column("ImageGuid");
            Map(x => x.Logo).Column("Logo").CustomSqlType("VARBINARY (MAX) FILESTREAM").Length(2147483647).LazyLoad();
            Map(x => x.ContactPersonDesignation).Column("ContactPersonDesignation");
            Map(x => x.ContactMobile).Column("ContactMobile");
            Map(x => x.ContactEmail).Column("ContactEmail");

            References(x => x.CompanyType).Column("CompanyTypeId");
            References(x => x.UserProfile).Column("UserProfileId");

            HasMany(x => x.CompanyDetailses).KeyColumn("CompanyId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobPosts).KeyColumn("CompanyId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.CompanyJobPosts).KeyColumn("CompanyId").Cascade.AllDeleteOrphan().Inverse();

        }
    }
}
