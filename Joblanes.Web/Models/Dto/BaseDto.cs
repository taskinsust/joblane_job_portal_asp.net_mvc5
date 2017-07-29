using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Joblanes.Models.Dto
{
    public class BaseDto
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Status { get; set; }     
        public virtual string CreateBy { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual string CreateDate { get; set; }
        public virtual string ModifyDate { get; set; }   
    }
}
