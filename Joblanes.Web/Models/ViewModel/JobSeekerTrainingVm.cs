using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobSeekerTrainingVm
    {
        public virtual long Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        public virtual string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public virtual string Description { get; set; }
        [Required(ErrorMessage = "Institute is required")]
        [Display(Name = "Institute")]
        public virtual string Institute { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        [Display(Name = "StartDate")]
        public virtual DateTime StartDate { get; set; }
        [Required(ErrorMessage = "CloseDate is required")]
        [Display(Name = "CloseDate")]
        public virtual DateTime CloseDate { get; set; }
        public virtual byte[] Cirtificate { get; set; }
        public virtual string AspNetuserId { get; set; }
    }
}