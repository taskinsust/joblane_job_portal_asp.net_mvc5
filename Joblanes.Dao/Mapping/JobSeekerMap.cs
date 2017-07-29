using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerMap: BaseClassMap<JobSeeker, long> 
    {
        public JobSeekerMap()
        {
            Table("JobSeeker");
            LazyLoad();

            Map(x => x.FirstName).Column("FirstName");
            Map(x => x.LastName).Column("LastName");
            Map(x => x.ImageGuid).Column("ImageGuid");
            Map(x => x.ProfileImage).Column("ProfileImage").CustomSqlType("VARBINARY (MAX) FILESTREAM").Length(2147483647).LazyLoad();
            Map(x => x.ContactNumber).Column("ContactNumber");
            Map(x => x.ContactEmail).Column("ContactEmail");

            References(x => x.UserProfile).Column("UserProfileId");

            HasMany(x => x.JobSeekerDetailses).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerEducationalQualifications).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerExperiences).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerTrainingCoursesList).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerJobPosts).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerDesiredJobs).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();

            HasMany(x => x.JobSeekerSkills).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerLinks).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerMilitarys).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerAwards).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerGroups).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerPatentses).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerPublicationses).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.JobSeekerAdditionalInformations).KeyColumn("JobSeekerId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
