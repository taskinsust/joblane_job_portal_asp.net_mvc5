using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;
using Model.JobLanes.Entity.User;

namespace Model.JobLanes.Entity
{
    public class JobSeekerPrivateSetting : BaseEntity<long>
    {
        public virtual JobSeeker JobSeeker { get; set; }
        public virtual bool IsMailShow { get; set; }
        public virtual bool IsPhoneShow { get; set; }
        public virtual bool IsPublicResume { get; set; }

    }
}
