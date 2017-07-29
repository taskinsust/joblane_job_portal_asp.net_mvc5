using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerCvBankDto
    {
        public virtual long Id { get; set; }
        public virtual long UserProfile { get; set; }
        public virtual Guid CvGuid { get; set; }
        public virtual byte[] Cv { get; set; }
    }
}
