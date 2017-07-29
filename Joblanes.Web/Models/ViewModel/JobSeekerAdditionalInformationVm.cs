using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobSeekerAdditionalInformationVm
    {
        public virtual long Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string JobSeeker { get; set; }
    }
}