using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class ProfileViewDto : BaseDto
    {
        public virtual JobSeekerDto JobSeeker { get; set; }
        public virtual CompanyDto Company { get; set; }
        public virtual bool? IsCompany { get; set; } 
    }
}
