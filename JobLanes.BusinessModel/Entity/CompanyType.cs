using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class CompanyType: BaseEntity<long> 
    {
        public CompanyType()
        {
            Companies = new List<Company>();
        }

        public virtual string Description { get; set; }

        public virtual IList<Company> Companies { get; set; }
    }
}
