using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Joblanes.Mapping
{
    public class RegionMap : BaseClassMap<Region, long> 
    {
        public RegionMap()
        {
            Table("Region");
            LazyLoad();

            Map(x => x.Name).Column("Name");

            HasMany(x => x.Countries).KeyColumn("RegionId").Cascade.AllDeleteOrphan().Inverse();

        }
    }
}
