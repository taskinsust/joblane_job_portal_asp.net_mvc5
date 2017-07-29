using System;
using System.Text;
using System.Collections.Generic;
using Model.JobLanes.Entity.Base;


namespace Model.JobLanes.Entity
{

    public class JobPost : BaseEntity<long>
    {
        public JobPost()
        {
            JobSeekerJobPosts = new List<JobSeekerJobPost>();
        }
        
        public virtual string JobTitle { get; set; }
        public virtual int? NoOfVacancies { get; set; }
        public virtual bool? IsOnlineCv { get; set; }
        public virtual bool? IsEmailCv { get; set; }
        public virtual bool? IsHardCopy { get; set; }
        public virtual bool? IsPhotographAttach { get; set; }
        public virtual string ApplyInstruction { get; set; }
        public virtual DateTime? DeadLine { get; set; }
        public virtual bool? IsDisplayCompanyName { get; set; }
        public virtual bool? IsDisplayCompanyAddress { get; set; }
        public virtual bool? IsDisplayCompanyBusiness { get; set; }
        public virtual int? AgeRangeFrom { get; set; }
        public virtual int? AgeRangeTo { get; set; }
        public virtual bool? IsMale { get; set; }
        public virtual bool? IsFemale { get; set; }
        public virtual int? JobLevel { get; set; }
        public virtual string EducationalQualification { get; set; }
        public virtual string JobDescription { get; set; }
        public virtual string AdditionalRequirements { get; set; }
        public virtual bool? IsExperienceRequired { get; set; }
        public virtual int? ExperienceMin { get; set; }
        public virtual int? ExperienceMax { get; set; }
        public virtual string JobLocation { get; set; }
        public virtual bool? IsShowSalary { get; set; }
        public virtual string SalaryRange { get; set; }
        public virtual int? SalaryMin { get; set; }
        public virtual int? SalaryMax { get; set; }
        public virtual string SalaryDetail { get; set; }
        public virtual string OtherBenefit { get; set; }
        public virtual Company Company { get; set; }
        public virtual long? PackageId { get; set; }
        public virtual JobCategory JobCategory { get; set; }
        public virtual JobType JobType { get; set; }
        public virtual IList<JobSeekerJobPost> JobSeekerJobPosts { get; set; }
    }
}
