using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerMilitaryServiceDto : BaseDto
    {
        public virtual string Branch { get; set; }
        public virtual string Rank { get; set; }
        public virtual bool IsStillServing { get; set; }
        public virtual DateTime DateFrom { get; set; }
        public virtual DateTime? DateTo { get; set; }
        public virtual string Description { get; set; }
        public virtual string Commendations { get; set; }
        public virtual CountryDto Country { get; set; }   
    }
}
