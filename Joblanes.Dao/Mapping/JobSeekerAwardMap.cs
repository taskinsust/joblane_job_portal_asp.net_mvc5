using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerAwardMap:BaseClassMap<JobSeekerAward,long>
    {
        public JobSeekerAwardMap()
        {
            Table("JobSeekerAward");
            LazyLoad();
            Map(x => x.DateAwarded).Column("DateAwarded");
            Map(x => x.Description).Column("Description");
            Map(x => x.Title).Column("Title");
            References(x => x.JobSeeker).Column("JobSeekerId");


        }
    }
}
