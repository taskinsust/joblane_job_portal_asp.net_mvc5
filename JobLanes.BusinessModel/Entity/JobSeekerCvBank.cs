using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;
using Model.JobLanes.Entity.User;

namespace Model.JobLanes.Entity
{
    public class JobSeekerCvBank:BaseEntity<long>
    {
        public virtual UserProfile UserProfile { get; set; }
        public virtual Guid CvGuid { get; set; }
        public virtual byte[] Cv { get; set; }
    }
}
