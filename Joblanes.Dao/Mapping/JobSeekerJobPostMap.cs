using System;
using System.Collections.Generic;
using System.Text;
using Dao.Joblanes.Mapping.Base;
using FluentNHibernate.Mapping;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{


    public class JobSeekerJobPostMap : BaseClassMap<JobSeekerJobPost, long>
    {

        public JobSeekerJobPostMap()
        {
            Table("JobSeekerJobPost");
            LazyLoad();
            Map(x => x.IsShortList).Column("IsShortList");
            Map(x => x.IsApplied).Column("IsApplied");
            Map(x => x.IsShortListedByCompany).Column("IsShortListedByCompany");
            References(x => x.JobSeeker).Column("JobSeekerId");
            References(x => x.JobPost).Column("JobPostId");
            
            
        }
    }
}
