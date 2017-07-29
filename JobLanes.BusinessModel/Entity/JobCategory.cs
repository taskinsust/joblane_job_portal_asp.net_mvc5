using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class JobCategory: BaseEntity<long> 
    {
        public JobCategory()
        {
        }

        public virtual string Description { get; set; }
    }
}
