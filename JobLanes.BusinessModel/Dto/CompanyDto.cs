using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class CompanyDto : BaseDto
    {

        public virtual string ContactPerson { get; set; }
        public virtual byte[] Logo { get; set; }
        public virtual string ContactPersonDesignation { get; set; }
        public virtual string ContactMobile { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual CompanyTypeDto CompanyType { get; set; }
        public virtual UserProfileDto UserProfile { get; set; }
        public virtual IList<CompanyDetailsDto> CompanyDetailList { get; set; }   
    }
}
