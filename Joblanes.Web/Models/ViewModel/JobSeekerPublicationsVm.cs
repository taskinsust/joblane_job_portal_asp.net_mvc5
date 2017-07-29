using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobSeekerPublicationsVm
    {
        private DateTime _returnDate = DateTime.Now;
        public virtual long Id { get; set; }
        [Required]
        public virtual string Title { get; set; }
        [Required]
        [Url]
        public virtual string Url { get; set; }
        [Required]
        public virtual DateTime PublishDate
        {
            get
            {
                return _returnDate;
            }
            set { _returnDate = value; }
        }
        [Required]
        public virtual string Description { get; set; }
        public virtual string JobSeeker { get; set; }
    }
}