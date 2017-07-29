using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class FindResumeFilterDto
    {
        public virtual string Title { get; set; }
        public virtual long TotalResume { get; set; }
        public virtual int Type { get; set; }
    }
}
