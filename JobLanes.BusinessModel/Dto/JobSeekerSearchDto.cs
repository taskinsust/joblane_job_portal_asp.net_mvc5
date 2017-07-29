using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerSearchDto
    {
        public virtual long RowNum { get; set; }
        public virtual string ContactEmail { get; set; }
         public virtual string ContactNumber { get; set; }
         public virtual string ZipCode { get; set; }
         public virtual string CompanyName { get; set; }
         public virtual string JobSeekerName { get; set; } 
         public virtual string Designation { get; set; }
        public virtual long JobSeekerId { get; set; }
        public virtual string JobSeekerLocation { get; set; } 
        public virtual DateTime? ExpFromDate {get; set; }
         public virtual DateTime? ExpToDate {get; set; } 
    }
}
