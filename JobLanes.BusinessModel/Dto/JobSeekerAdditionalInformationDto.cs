using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerAdditionalInformationDto
    {
        public virtual long Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string JobSeeker { get; set; }
    }
}
