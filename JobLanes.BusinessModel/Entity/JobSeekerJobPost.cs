using System;
using System.Text;
using System.Collections.Generic;
using Model.JobLanes.Entity.Base;


namespace Model.JobLanes.Entity
{

    public class JobSeekerJobPost : BaseEntity<long>
    {
        public virtual JobSeeker JobSeeker { get; set; }
        public virtual JobPost JobPost { get; set; }
        public virtual bool IsShortList { get; set; }
        public virtual bool IsApplied { get; set; }
        public virtual bool IsShortListedByCompany { get; set; }
    }
}
