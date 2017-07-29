using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;

namespace Model.JobLanes.Entity
{
    public class JobSeekerLink:BaseEntity<long>
    {
        public virtual string Link { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }
    }
}
