using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobSeekerAwardVm
    {
        private DateTime _returnDate = DateTime.Now;
        public virtual long Id { get; set; }
        [Required(ErrorMessage = "Please enter Title")]
        public virtual string Title { get; set; }

        [Required(ErrorMessage = "Please enter Date")]
        [Display(Name = "Date Awarded")]
        //[BaseVm.ValidateDateRange]
        public virtual DateTime DateAwarded
        {
            get
            {
                return _returnDate;
            }
            set { _returnDate = value; }
        }
        [Display(Name = "Degree")]
        public virtual string Description { get; set; }

        [Required(ErrorMessage = "Please enter Description")]
        public virtual string JobSeeker { get; set; }

        }
}