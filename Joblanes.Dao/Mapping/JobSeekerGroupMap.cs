using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerGroupMap : BaseClassMap<JobSeekerGroup, long>
    {
        public JobSeekerGroupMap()
        {
            Table("JobSeekerGroup");
            LazyLoad();
            References(x => x.JobSeeker).Column("JobSeekerId");
            Map(x => x.Title).Column("Title");
            Map(x => x.IsStillMember).Column("IsStillMember");
            Map(x => x.DateFrom).Column("DateFrom");
            Map(x => x.DateTo).Column("DateTo");
            Map(x => x.Description).Column("Description");
            
        }
    }
}
