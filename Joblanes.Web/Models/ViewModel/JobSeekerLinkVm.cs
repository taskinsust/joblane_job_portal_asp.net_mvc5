using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobSeekerLinkVm
    {
        public virtual long Id { get; set; }
        [Required]
        [Url]
        public virtual string Link { get; set; }
        public virtual string JobSeeker { get; set; }
    }
}