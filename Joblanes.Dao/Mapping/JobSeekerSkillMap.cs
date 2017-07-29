using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerSkillMap:BaseClassMap<JobSeekerSkill,long>
    {
        public JobSeekerSkillMap()
        {
            Table("JobSeekerSkill");
            LazyLoad();
            References(x => x.JobSeeker).Column("JobSeekerId");
            Map(x => x.Skill).Column("Skill");
            Map(x => x.Experence).Column("Experence");
        }
    }
}
