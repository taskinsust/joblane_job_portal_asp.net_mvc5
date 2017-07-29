using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class CountryMap: BaseClassMap<Country, long> 
    {
        public CountryMap()
        {
            Table("Country");
            LazyLoad();

            Map(x => x.Name).Column("Name");
            Map(x => x.ShortName).Column("ShortName");
            Map(x => x.ImageGuid).Column("ImageGuid");
            Map(x => x.Flag).Column("Flag").CustomSqlType("VARBINARY (MAX) FILESTREAM").Length(2147483647).LazyLoad();
            Map(x => x.CallingCode).Column("CallingCode");

            References(x => x.Region).Column("RegionId");

            HasMany(x => x.Cities).KeyColumn("CountryId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.States).KeyColumn("CountryId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.CompanyDetailses).KeyColumn("CountryId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerDetailses).KeyColumn("CountryId").Cascade.AllDeleteOrphan().Inverse();


        }
    }
}
