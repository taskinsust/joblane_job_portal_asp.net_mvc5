using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerLinkMap:BaseClassMap<JobSeekerLink,long>
    {
        public JobSeekerLinkMap()
        {
            Table("JobSeekerLink");
            LazyLoad();
            References(x => x.JobSeeker).Column("JobSeekerId");
            Map(x => x.Link).Column("Link");
        }
    }
}
