using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerSkillDto
    {
        public virtual long Id { get; set; }
        public virtual string JobSeeker { get; set; }
        public virtual string Skill { get; set; }
        public virtual int Experence { get; set; }
    }
}
