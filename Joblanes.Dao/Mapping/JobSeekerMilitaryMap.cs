using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerMilitaryMap : BaseClassMap<JobSeekerMilitary, long>
    {
        public JobSeekerMilitaryMap()
        {
            Table("JobSeekerMilitary");
            LazyLoad();
            References(x => x.JobSeeker).Column("JobSeekerId");
            References(x => x.Country).Column("CountryId");
            Map(x => x.Branch).Column("Branch");
            Map(x => x.Rank).Column("Rank");
            Map(x => x.IsStillServing).Column("IsStillServing");
            Map(x => x.DateFrom).Column("DateFrom");
            Map(x => x.DateTo).Column("DateTo");
            Map(x => x.Description).Column("Description");
            Map(x => x.Commendations).Column("Commendations");

        }
    }
}
