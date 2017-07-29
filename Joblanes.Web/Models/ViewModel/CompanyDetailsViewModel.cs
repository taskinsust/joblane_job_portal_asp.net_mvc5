using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Joblanes.Models.ViewModel
{
    public class CompanyDetailsViewModel
    {
        public virtual string TradeLicence { get; set; }
        public virtual string WebLink { get; set; }
        public virtual string LinkdinLink { get; set; }
        public virtual string Vision { get; set; }
        public virtual string Mission { get; set; }
        public virtual string Description { get; set; }
        public virtual string Address { get; set; }
        public virtual string Zip { get; set; }
        public virtual string TagLine { get; set; }
        public virtual long Country { get; set; }
        public virtual long State { get; set; }
        public virtual long City { get; set; }
        public virtual long Company { get; set; }     
    }
}
