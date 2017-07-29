using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class PaymentType: BaseEntity<long> 
    {
        public PaymentType()
        {
        }

        public virtual string Description { get; set; }
    }
}
