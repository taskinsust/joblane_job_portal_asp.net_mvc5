using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobSeekerGroupVm
    {
        private DateTime _returnDate = DateTime.Now;
        //private DateTime _returnDateTo = DateTime.Now;
        public virtual long Id { get; set; }
        [Required]
        public virtual string Title { get; set; }
        public virtual bool IsStillMember { get; set; }
        [Required]
        //[BaseVm.ValidateDateRange]
        public virtual DateTime DateFrom
        {
            get
            {
                return _returnDate;
            }
            set { _returnDate = value; }
        }
        //[BaseVm.ValidateDateRange]
        public virtual DateTime? DateTo { get; set; }
        public virtual string Description { get; set; }
        public virtual string JobSeeker { get; set; }
    }
}