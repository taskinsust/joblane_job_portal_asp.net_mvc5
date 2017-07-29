using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Web.Joblanes.Models.ViewModel
{
    public class CountryViewModel 
    {
        public virtual long Id { get; set; }

        [Required]
        public virtual string Name { get; set; }
        public virtual int Status { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string CallingCode { get; set; }

        [Required]
        public virtual long Region { get; set; }

        public virtual byte[] FlagBytes { get; set; } 
        //  [NonSerialized]
        private HttpPostedFileBase _flag;  

        [DataType(DataType.Upload)]
        [DataMember]
        public HttpPostedFileBase Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
    }
}
