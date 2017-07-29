using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobSeekerExpVm
    {
        public virtual long Id { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [Display(Name = "Company")]
        public virtual string CompanyName { get; set; }
        [Display(Name = "Address")]
        public virtual string CompanyAddress { get; set; }
        [Required(ErrorMessage = "Job Title is required")]
        [Display(Name = "Job Title")]
        public virtual string Designation { get; set; }
        [Required(ErrorMessage = "Date From is required")]
        [Display(Name = "Date From")]
        //[BaseVm.ValidateDateRange]
        public virtual DateTime DateFrom { get; set; }
        //[BaseVm.ValidateDateRange]
        public virtual DateTime? DateTo { get; set; }
        [Display(Name = "Description")]
        public virtual string Responsibility { get; set; }
        [Display(Name = "I currently work here")]
        public virtual bool IsCurrent { get; set; }
        public virtual string AspNetuserId { get; set; }
    }
}