using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.ViewModel;

namespace Model.JobLanes.Dto
{
    public class JobSeekerProfileDto
    {
        public virtual JobSeekerProfileVm JobSeekerProfileVm { get; set; }
        public virtual IList<JobSeekerEduVm> JobSeekerEduVm { get; set; }
        public virtual IList<JobSeekerExpVm> JobSeekerExpVm { get; set; }
        public virtual IList<JobSeekerTrainingVm> JobSeekerTrainingVm { get; set; }
    }
}
