using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;

namespace Model.JobLanes.Dto
{
    public class JobSeekerCertificateDto
    {
        public virtual long Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Institute { get; set; }
        //[ValidateDateRange]
        public virtual DateTime StartDate { get; set; }
        //[ValidateDateRange]
        public virtual DateTime CloseDate { get; set; }
        public virtual Guid ImageGuid { get; set; }
        public virtual byte[] Cirtificate { get; set; }
        public virtual string JobSeeker { get; set; }
    }
}
