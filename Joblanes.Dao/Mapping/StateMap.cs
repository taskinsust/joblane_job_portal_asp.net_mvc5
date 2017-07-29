using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    class StateMap: BaseClassMap<State, long> 
    {
        public StateMap()
        {
            Table("State");
            LazyLoad();

            Map(x => x.Name).Column("Name");
            Map(x => x.ShortName).Column("ShortName");

            References(x => x.Country).Column("CountryId");

            HasMany(x => x.Cities).KeyColumn("StateId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.CompanyDetailses).KeyColumn("StateId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerDetailses).KeyColumn("StateId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
