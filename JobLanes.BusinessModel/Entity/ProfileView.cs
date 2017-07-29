using System;
using System.Text;
using System.Collections.Generic;
using Model.JobLanes.Entity.Base;


namespace Model.JobLanes.Entity
{

    public class ProfileView : BaseEntity<long>
    {
        public virtual JobSeeker JobSeeker { get; set; }
        public virtual Company Company { get; set; }
        public virtual bool? IsCompany { get; set; } 
    }
}
