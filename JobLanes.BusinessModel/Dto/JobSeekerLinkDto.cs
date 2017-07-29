using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerLinkDto
    {
        public virtual long Id { get; set; }
        public virtual string Link { get; set; }
        public virtual string JobSeeker { get; set; }
    }
}
