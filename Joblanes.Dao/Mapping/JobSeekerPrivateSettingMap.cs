using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using FluentNHibernate.Conventions;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerPrivateSettingMap : BaseClassMap<JobSeekerPrivateSetting, long>
    {
        public JobSeekerPrivateSettingMap()
        {
            Table("JobSeekerPrivateSetting");
            LazyLoad();

            Map(x => x.IsMailShow).Column("IsMailShow");
            Map(x => x.IsPhoneShow).Column("IsPhoneShow");
            Map(x => x.IsPublicResume).Column("IsPublicResume");
            References(x => x.JobSeeker).Column("JobSeekerId");
        }
    }
}
