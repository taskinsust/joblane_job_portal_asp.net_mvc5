using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobApplicationDto : BaseDto
    {
        public virtual JobPostsDto JobPost { get; set; }
        public virtual JobSeekerDto JobSeeker { get; set; }  
        public virtual bool IsShortList { get; set; }
        public virtual bool IsApplied { get; set; }
        public virtual bool IsShortListedByCompany { get; set; }
    }
}
