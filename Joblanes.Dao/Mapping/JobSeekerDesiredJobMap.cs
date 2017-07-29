using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerDesiredJobMap : BaseClassMap<JobSeekerDesiredJob, long>
    {
        public JobSeekerDesiredJobMap()
        {
            Table("JobSeekerDesiredJob");
            LazyLoad();
            Map(x => x.DesiredSalary).Column("DesiredSalary");
            Map(x => x.SalaryDurationInDay).Column("SalaryDurationInDay");
            Map(x => x.IsRelocate).Column("IsRelocate");
            Map(x => x.EmploymentEligibility).Column("EmploymentEligibility");
            Map(x => x.IsPartTime).Column("IsPartTime");
            Map(x => x.IsInternship).Column("IsInternship");
            Map(x => x.IsTemporary).Column("IsTemporary");
            Map(x => x.IsFullTime).Column("IsFullTime");
            Map(x => x.IsCommission).Column("IsCommission");
            Map(x => x.IsContract).Column("IsContract");
            Map(x => x.RelocatingPlaceOne).Column("RelocatingPlaceOne");
            Map(x => x.RelocatingPlaceTwo).Column("RelocatingPlaceTwo");
            Map(x => x.RelocatingPlaceThree).Column("RelocatingPlaceThree");
            References(x => x.JobSeeker).Column("JobSeekerId");

        }
    }
}
