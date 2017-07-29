using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobSeekerEduVm
    {
        public virtual long Id { get; set; }
        [Required(ErrorMessage = "School is required")]
        [Display(Name = "School")]
        public virtual string Institute { get; set; }

        [Required(ErrorMessage = "Degree is required")]
        [Display(Name = "Degree")]
        public virtual string Degree { get; set; }

        [Required(ErrorMessage = "Field Of Study is required")]
        [Display(Name = "Field Of Study")]
        public virtual string FieldOfStudy { get; set; }

        [Required(ErrorMessage = "Starting Year is required")]
        [Display(Name = "Starting Year")]
        //[BaseVm.ValidateDateRange]
        public virtual DateTime StartingYear { get; set; }
        [Display(Name = "Passing Year")]
        //[BaseVm.ValidateDateRange]
        public virtual DateTime PassingYear { get; set; }
        public virtual byte[] Cirtificate { get; set; }
        public virtual float Result { get; set; }
        public virtual string AspNetuserId { get; set; }
    }
}