using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerCvBankMap : BaseClassMap<JobSeekerCvBank, long>
    {
        public JobSeekerCvBankMap()
        {
            Table("JobSeekerCvBank");
            LazyLoad();
            Map(x => x.CvGuid).Column("CvGuid");
            Map(x => x.Cv).Column("Cv").CustomSqlType("VARBINARY (MAX) FILESTREAM").Length(2147483647).LazyLoad();
            References(x => x.UserProfile).Column("UserProfileId");
        }
    }
}
