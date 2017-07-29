using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerExperienceDto : BaseDto
    {
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
