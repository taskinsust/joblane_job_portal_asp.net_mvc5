using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class JobSeekerEducationalQualification : BaseEntity<long>
    {
        public JobSeekerEducationalQualification()
        {

        }

        public virtual string Degree { get; set; }
        public virtual string Institute { get; set; }
        public virtual string FieldOfStudy { get; set; }
        //[ValidateDateRange]
        public virtual DateTime StartingYear { get; set; }
        //[ValidateDateRange]
        public virtual DateTime PassingYear { get; set; }
        public virtual Guid ImageGuid { get; set; }
        public virtual byte[] Cirtificate { get; set; }
        public virtual float Result { get; set; }

        public virtual JobSeeker JobSeeker { get; set; }

    }
}
