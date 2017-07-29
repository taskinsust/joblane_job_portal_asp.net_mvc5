using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.JobLanes.ViewModel
{
    public class CompanyViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Company Name is required")]
        [Display(Name = "Company Name")]
        [StringLength(200)]
        public virtual string CompanyName { get; set; }
        public virtual long Id { get; set; }

        [Display(Name = "Contact Person")]
        [StringLength(150)]
        public virtual string ContactPerson { get; set; }

        [Display(Name = "Designation")]
        [StringLength(50)]
        public virtual string ContactPersonDesignation { get; set; }

        [Required(ErrorMessage = "Contact Mobile is required")]
        [Display(Name = "Contact Mobile")]
        [StringLength(50)]
        public virtual string ContactMobile { get; set; }

        [Required(ErrorMessage = "Contact Email is required")]
        [Display(Name = "Contact Email")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email ")]
        public virtual string ContactEmail { get; set; }

        [Display(Name = "Contact Mobile")]
        public virtual byte[] LogoBytes { get; set; }
        //  [NonSerialized]
        private HttpPostedFileBase _logo;

        [DataType(DataType.Upload)]
        [DataMember]
        public HttpPostedFileBase Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }

        public virtual string UserProfileId { get; set; }

        [Required(ErrorMessage = "Company Type is required")]
        [Display(Name = "Company Type")]
        public virtual long CompanyType { get; set; }

        [Display(Name = "Trade Licence")]
        [StringLength(200)]
        public virtual string TradeLicence { get; set; }

        [Display(Name = "Website")]
        [StringLength(200)]
        public virtual string WebLink { get; set; }

        [Display(Name = "Linkdin Link")]
        [StringLength(200)]
        public virtual string LinkdinLink { get; set; }

        [Display(Name = "Vision")]
        public virtual string Vision { get; set; }

        [Display(Name = "Mission")]
        public virtual string Mission { get; set; }

        [Display(Name = "Description")]
        public virtual string Description { get; set; }

        [Display(Name = "Address")]
        public virtual string Address { get; set; }

        [Display(Name = "Zip")]
        [StringLength(20)]
        //[Range(1000, 9999, ErrorMessage = "The Zip must be 4 characters long.")]  
        public virtual string Zip { get; set; }

        [Display(Name = "Tag Line")]
        public virtual string TagLine { get; set; }

        [Display(Name = "Region")]
        public virtual long Region { get; set; }

        [Display(Name = "Country")]
        public virtual long Country { get; set; }

        [Display(Name = "State")]
        public virtual long? State { get; set; }

        [Display(Name = "City")]
        public virtual long City { get; set; }

        [Display(Name = "Employee Size")]
        public virtual int EmployeeSize { get; set; }

        [Display(Name = "Established Date")]
        public virtual DateTime? EstablishedDate { get; set; }
   
        //public virtual long Id { get; set; }
        //public virtual string CompanyName { get; set; } 
        //public virtual string ContactPerson { get; set; }
        //public virtual string ContactPersonDesignation { get; set; }
        //public virtual string ContactMobile { get; set; }
        //public virtual string ContactEmail { get; set; }
        //public virtual byte[] LogoBytes { get; set; }
        //public virtual string UserProfileId { get; set; }
        //public virtual long CompanyType { get; set; }
        //public virtual string TradeLicence { get; set; }
        //public virtual string WebLink { get; set; }
        //public virtual string LinkdinLink { get; set; }
        //public virtual string Vision { get; set; }
        //public virtual string Mission { get; set; }
        //public virtual string Description { get; set; }
        //public virtual string Address { get; set; }
        //public virtual string Zip { get; set; }
        //public virtual string TagLine { get; set; }
        //public virtual long Region { get; set; } 
        //public virtual long Country { get; set; }
        //public virtual long State { get; set; }
        //public virtual long City { get; set; }
        //public virtual int EmployeeSize { get; set; }
        //public virtual DateTime? EstablishedDate { get; set; }
    }
    
}
