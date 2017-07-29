using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace JobLanes.WcfService.Models
{

   
    public class ResponseMessage
    {
      
        public virtual bool IsSuccess { get; set; }

        public virtual string Type { get; set; } 
      
        public virtual string Message { get; set; }

      
        public virtual string AdditionalValue { get; set; }
    }
}