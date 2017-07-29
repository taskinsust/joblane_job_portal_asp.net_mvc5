using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.JobLanes.ViewModel
{
    public class JobSeekerTrainingVm
    {
        public virtual long Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Institute { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime CloseDate { get; set; }
        public virtual byte[] Cirtificate { get; set; }
        public virtual string AspNetuserId { get; set; }
    }
}