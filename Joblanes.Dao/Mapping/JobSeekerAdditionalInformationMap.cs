using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerAdditionalInformationMap : BaseClassMap<JobSeekerAdditionalInformation, long> 
    {
        public JobSeekerAdditionalInformationMap()
        {
            Table("JobSeekerAdditionalInformation");
            LazyLoad();
            Map(x => x.Description);
            References(x => x.JobSeeker).Column("JobSeekerId");
        }
    }
}
