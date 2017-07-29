using System;
using System.Collections.Generic;
using System.Text;
using Dao.Joblanes.Mapping.Base;
using FluentNHibernate.Mapping;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{


    public class JobPostMap : BaseClassMap<JobPost, long>
    {

        public JobPostMap()
        {
            Table("JobPost");
            LazyLoad();

            Map(x => x.PackageId).Column("PackageId");
            Map(x => x.JobTitle).Column("JobTitle");
            Map(x => x.NoOfVacancies).Column("NoOfVacancies");
            Map(x => x.IsOnlineCv).Column("IsOnlineCv");
            Map(x => x.IsEmailCv).Column("IsEmailCv");
            Map(x => x.IsHardCopy).Column("IsHardCopy");
            Map(x => x.IsPhotographAttach).Column("IsPhotographAttach");
            Map(x => x.ApplyInstruction).Column("ApplyInstruction");
            Map(x => x.DeadLine).Column("DeadLine");
            Map(x => x.IsDisplayCompanyName).Column("IsDisplayCompanyName");
            Map(x => x.IsDisplayCompanyAddress).Column("IsDisplayCompanyAddress");
            Map(x => x.IsDisplayCompanyBusiness).Column("IsDisplayCompanyBusiness");
            Map(x => x.AgeRangeFrom).Column("AgeRangeFrom");
            Map(x => x.AgeRangeTo).Column("AgeRangeTo");
            Map(x => x.IsMale).Column("IsMale");
            Map(x => x.IsFemale).Column("IsFemale");
            Map(x => x.JobLevel).Column("JobLevel");
            Map(x => x.EducationalQualification).Column("EducationalQualification");
            Map(x => x.JobDescription).Column("JobDescription");
            Map(x => x.AdditionalRequirements).Column("AdditionalRequirements");
            Map(x => x.IsExperienceRequired).Column("IsExperienceRequired");
            Map(x => x.ExperienceMin).Column("ExperienceMin");
            Map(x => x.ExperienceMax).Column("ExperienceMax");
            Map(x => x.JobLocation).Column("JobLocation");
            Map(x => x.IsShowSalary).Column("IsShowSalary");
            Map(x => x.SalaryRange).Column("SalaryRange");
            Map(x => x.SalaryMin).Column("SalaryMin");
            Map(x => x.SalaryMax).Column("SalaryMax");
            Map(x => x.SalaryDetail).Column("SalaryDetail");
            Map(x => x.OtherBenefit).Column("OtherBenefit");

            
            References(x => x.Company).Column("CompanyId");
            References(x => x.JobCategory).Column("JobCategoryId");
            References(x => x.JobType).Column("JobTypeId");
            //References(x => x.company).KeyColumn("JobPostId");
            //HasMany(x => x.JobSeekerJobPost).KeyColumn("JobPostId");
        }
    }
}
