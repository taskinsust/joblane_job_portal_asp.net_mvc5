using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerExperienceMap : BaseClassMap<JobSeekerExperience, long>
    {
        public JobSeekerExperienceMap()
        {
            Table("JobSeekerExperience");
            LazyLoad();

            Map(x => x.CompanyName).Column("CompanyName");
            Map(x => x.CompanyAddress).Column("CompanyAddress");
            Map(x => x.Designation).Column("Designation");
            Map(x => x.DateFrom).Column("DateFrom");
            Map(x => x.DateTo).Column("DateTo");
            Map(x => x.Responsibility).Column("Responsibility");
            Map(x => x.IsCurrent).Column("IsCurrent");
            References(x => x.JobSeeker).Column("JobSeekerId");
        }
    }
}
