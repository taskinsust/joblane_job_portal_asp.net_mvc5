using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.JobLanes.ViewModel
{
    public class JobSeekerExpVm
    {
        public virtual long Id { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string CompanyAddress { get; set; }
        public virtual string Designation { get; set; }
        public virtual DateTime DateFrom { get; set; }
        public virtual DateTime? DateTo { get; set; }
        public virtual string Responsibility { get; set; }
        public virtual bool IsCurrent { get; set; }
        public virtual string AspNetuserId { get; set; }
    }
}