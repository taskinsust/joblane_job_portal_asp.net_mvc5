using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;

namespace Model.JobLanes.Dto
{
    public class JobSeekerMilitaryDto
    {
        public virtual long Id { get; set; }
        public virtual long Country { get; set; }
        public virtual string Branch { get; set; }
        public virtual string Rank { get; set; }
        public virtual bool IsStillServing { get; set; }
        //[ValidateDateRange]
        public virtual DateTime DateFrom { get; set; }
        //[ValidateDateRange]
        public virtual DateTime? DateTo { get; set; }
        public virtual string Description { get; set; }
        public virtual string Commendations { get; set; }
        public virtual string JobSeeker { get; set; }
    }
}
