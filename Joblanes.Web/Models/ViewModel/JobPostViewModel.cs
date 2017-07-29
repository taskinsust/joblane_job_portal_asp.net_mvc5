using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Joblanes.Context;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobPostViewModel
    {
        public virtual long Id { get; set; }
        
        public virtual string Name { get; set; }
        public virtual int Status { get; set; }

        [Display(Name = "Job Title")]
        [Required]
        public virtual string JobTitle { get; set; }

        [Display(Name = "No Of Vacancies")]
        [Range(1, Int32.MaxValue)]
        public virtual int? NoOfVacancies { get; set; }
        [Display(Name = "Is Online Cv")]
        public virtual bool IsOnlineCv { get; set; }
        [Display(Name = "Is Email Cv")]
        public virtual bool IsEmailCv { get; set; }
        [Display(Name = "Is Hard Copy")]
        public virtual bool IsHardCopy { get; set; }
        [Display(Name = "Is Photograph Attach")]
        public virtual bool IsPhotographAttach { get; set; }
        [Display(Name = "Apply Instruction")]
        public virtual string ApplyInstruction { get; set; }
        [Required]
        public virtual DateTime? DeadLine { get; set; }
        [Display(Name = "Is Display Company Name")]
        public virtual bool? IsDisplayCompanyName { get; set; }
        [Display(Name = "Is Display Company Address")]
        public virtual bool? IsDisplayCompanyAddress { get; set; }
        [Display(Name = "Is Display Company Business")]
        public virtual bool? IsDisplayCompanyBusiness { get; set; }
        [Display(Name = "Age Range From")]
        [Range(0, 100)]
        public virtual int? AgeRangeFrom { get; set; }
        [Display(Name = "Age Range To")]
        [Range(0, 100)]
        public virtual int? AgeRangeTo { get; set; }
        [Display(Name = "Male Only")]
        public virtual bool? IsMale { get; set; }
        [Display(Name = "Female Only")]
        public virtual bool? IsFemale { get; set; }
        public virtual int? Gender { get; set; }
        [Display(Name = "Job Level")]
        public virtual int? JobLevel { get; set; }
        [Display(Name = "Educational Qualification")]
        public virtual string EducationalQualification { get; set; }
        [Display(Name = "Job Description")]
        public virtual string JobDescription { get; set; }
        [Display(Name = "Additional Requirements")]
        public virtual string AdditionalRequirements { get; set; }
        [Display(Name = "Is Experience Required")]
        public virtual bool? IsExperienceRequired { get; set; }
        [Display(Name = "Experience Min")]
        [Range(0, 100)]
        public virtual int? ExperienceMin { get; set; }
        [Display(Name = "Experience Max")]
        [Range(0, 100)]
        public virtual int? ExperienceMax { get; set; }
        [Display(Name = "Job Location")]
        [Required]
        public virtual string JobLocation { get; set; }
        [Display(Name = "Is Show Salary")]
        public virtual bool? IsShowSalary { get; set; }
        [Display(Name = "Salary Range")]
        public virtual string SalaryRange { get; set; }
        [Display(Name = "Minimum Salary")]
        [Range(0, Int32.MaxValue)]
        public virtual int? SalaryMin { get; set; }
        [Display(Name = "Maximum Salary")]
        [Range(0, Int32.MaxValue)]
        public virtual int? SalaryMax { get; set; }
        [Display(Name = "Salary Detail")]
        public virtual string SalaryDetail { get; set; }
        [Display(Name = "Other Benefit")]
        public virtual string OtherBenefit { get; set; }
        public virtual long Company { get; set; }
        public virtual long? PackageId { get; set; }
        [Display(Name = "Job Category")]
        public virtual long JobCategory { get; set; }
        [Required]
        [Display(Name = "Job Type")]
        public virtual long JobType { get; set; }
    }
}
