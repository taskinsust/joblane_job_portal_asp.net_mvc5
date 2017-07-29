using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerEducationDto : BaseDto
    {
        public virtual string Degree { get; set; }
        public virtual string Institute { get; set; }
        public virtual string FieldOfStudy { get; set; }
        public virtual DateTime StartingYear { get; set; }
        public virtual DateTime PassingYear { get; set; }
        public virtual float Result { get; set; }
    }
}
