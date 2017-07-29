using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class JobSeekerDetails : BaseEntity<long>
    {
        public JobSeekerDetails()
        {
        }

        public virtual string FatherName { get; set; }
        public virtual string MotherName { get; set; }
        public virtual string Address { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string Linkedin { get; set; }
        public virtual string Weblink { get; set; }
        public virtual Guid ImageGuid { get; set; }
        public virtual byte[] Cv { get; set; }
        public virtual DateTime Dob { get; set; }
        public virtual int Gender { get; set; }
        public virtual int MaritalStatus { get; set; }
        public virtual string Expertise { get; set; }

        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual City City { get; set; }
        public virtual Region Region { get; set; }

        public virtual JobSeeker JobSeeker { get; set; }

    }
}
