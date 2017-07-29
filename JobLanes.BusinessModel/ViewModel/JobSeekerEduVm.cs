using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.JobLanes.ViewModel
{
    public class JobSeekerEduVm
    {
        public virtual long Id { get; set; }
        public virtual string Institute { get; set; }
        public virtual string FieldOfStudy { get; set; }
        public virtual string Degree { get; set; }
        public virtual DateTime StartingYear { get; set; }
        public virtual DateTime PassingYear { get; set; }
        public virtual byte[] Cirtificate { get; set; }
        public virtual float Result { get; set; }
        public virtual string AspNetuserId { get; set; }
    }
}