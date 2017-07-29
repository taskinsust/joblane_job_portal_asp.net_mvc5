using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Model.JobLanes.ViewModel
{
    //[Serializable]
    [DataContract]
    public class JobSeekerProfileVm
    {
        //public virtual long Id { get; set; }

        //[Required(ErrorMessage = "Please Select FirstName")]
        //[Display(Name = "First Name")]
        //[DataMember]
        //public virtual string FirstName { get; set; }
        //[Required(ErrorMessage = "Please Select LastName")]
        //[Display(Name = "Last Name")]
        //[DataMember]
        //public virtual string LastName { get; set; }
        //[Required(ErrorMessage = "Please Enter ContactNumber")]
        //[Display(Name = "Contact Number")]
        //[DataMember]
        //public virtual string ContactNumber { get; set; }
        //[Required(ErrorMessage = "Please Enter ContactEmail")]
        //[Display(Name = "Contact Email")]
        //[EmailAddress(ErrorMessage = "Invalid Email ")]
        //[DataMember]
        //public virtual string ContactEmail { get; set; }
        ////[Required(ErrorMessage = "Please Enter FatherName")]
        //[Display(Name = "Father's Name")]
        //[DataMember]
        //public virtual string FatherName { get; set; }
        ////[Required(ErrorMessage = "Please Enter MotherName")]
        //[Display(Name = "Mother's Name")]
        //[DataMember]
        //public virtual string MotherName { get; set; }
        //[Required(ErrorMessage = "Please Enter Address")]
        //[Display(Name = "Address")]
        //[DataMember]
        //public virtual string Address { get; set; }
        //[Required(ErrorMessage = "Please Enter ZipCode")]
        //[Display(Name = "Zip Code")]
        //[DataMember]
        //public virtual string ZipCode { get; set; }
        //[Required(ErrorMessage = "Please Enter Linkedin")]
        //[Display(Name = "Linkedin")]
        //[DataMember]
        //public virtual string Linkedin { get; set; }
        //[Display(Name = "Web link")]
        //[DataMember]
        //public virtual string Weblink { get; set; }
        //[Required(ErrorMessage = "Please Enter Dob")]
        //[Display(Name = "Date of Birth")]
        //[DataMember]

        //public virtual DateTime Dob { get; set; }

        //[Required(ErrorMessage = "Please Enter Gender")]
        //[Display(Name = "Gender")]
        //[DataMember]
        //public virtual int Gender { get; set; }
        //[Required(ErrorMessage = "Please Select MaritalStatus")]
        //[Display(Name = "Marital Status")]
        //[DataMember]
        //public virtual int MaritalStatus { get; set; }
        ////[Required(ErrorMessage = "Please Enter Expertise")]
        //[Display(Name = "Expertise")]
        //[DataMember]
        //public virtual string Expertise { get; set; }
        //public virtual byte[] ProfileImageBytes { get; set; }
        ////  [NonSerialized]
        ////private HttpPostedFileBase _profileImage;

        //[DataType(DataType.Upload)]
        //[DataMember]
        //public HttpPostedFileBase ProfileImage
        //{
        //    get;
        //    set;
        //}

        //// [NonSerialized]
        ////private HttpPostedFileBase _cv;

        //[DataType(DataType.Upload)]
        //[DataMember]
        //public HttpPostedFileBase Cv
        //{
        //    get;
        //    set;
        //}
        //[Required(ErrorMessage = "Please Select Region")]
        //[Display(Name = "Region")]
        //[DataMember]
        //public virtual long RegionId { get; set; }

        //[Required(ErrorMessage = "Please Select Country")]
        //[Display(Name = "Country")]
        //[DataMember]
        //public virtual long CountryId { get; set; }
        //[Required(ErrorMessage = "Please Select State")]
        //[Display(Name = "State")]
        //[DataMember]
        //public virtual long StateId { get; set; }
        //[Required(ErrorMessage = "Please Select City")]
        //[Display(Name = "City")]
        //[DataMember]
        //public virtual long CityId { get; set; }
        //[DataMember]
        //public virtual string UserProfileId { get; set; }
        //public virtual bool IsPublicResume { get; set; }
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string FirstName { get; set; }
        [DataMember]
        public virtual string LastName { get; set; }
        [DataMember]
        public virtual string ContactNumber { get; set; }
        [DataMember]
        public virtual string ContactEmail { get; set; }
        [DataMember]
        public virtual string FatherName { get; set; }
        [DataMember]
        public virtual string MotherName { get; set; }
        [DataMember]
        public virtual string Address { get; set; }
        [DataMember]
        public virtual string ZipCode { get; set; }
        [DataMember]
        public virtual string Linkedin { get; set; }
        [DataMember]
        public virtual string Weblink { get; set; }
        [DataMember]
        public virtual DateTime Dob { get; set; }
        [DataMember]
        public virtual int Gender { get; set; }
        [DataMember]
        public virtual int MaritalStatus { get; set; }
        [DataMember]
        public virtual string Expertise { get; set; }
        [DataMember]
        public virtual byte[] ProfileImageBytes { get; set; }
        [DataMember]
        public virtual byte[] CvBytes { get; set; }
        [DataMember]
        public virtual long RegionId { get; set; }
        [DataMember]
        public virtual long CountryId { get; set; }
        [DataMember]
        public virtual long StateId { get; set; }
        [DataMember]
        public virtual long CityId { get; set; }
        [DataMember]
        public virtual string UserProfileId { get; set; }
        [DataMember]
        public virtual bool IsPublicResume { get; set; }

    }
}