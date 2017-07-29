using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
   public class CityMap: BaseClassMap<City, long> 
    {
       public CityMap()
        {
            Table("City");
            LazyLoad();

            Map(x => x.Name).Column("Name");
            Map(x => x.ShortName).Column("ShortName");
            References(x => x.Country).Column("CountryId");
            References(x => x.State).Column("StateId");

            HasMany(x => x.CompanyDetailses).KeyColumn("CityId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerDetailses).KeyColumn("CityId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
