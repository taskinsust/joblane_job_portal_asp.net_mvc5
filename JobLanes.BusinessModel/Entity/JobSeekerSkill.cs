using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;

namespace Model.JobLanes.Entity
{
    public class JobSeekerSkill:BaseEntity<long>
    {
        public virtual JobSeeker JobSeeker { get; set; }
        public virtual string Skill { get; set; }
        public virtual int Experence { get; set; }
    }
}
