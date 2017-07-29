using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Models.ViewModel
{
    public class JobSeekerCertificateVm
    {
        private DateTime _returnDate = DateTime.Now;
        private DateTime _returnCloseDate = DateTime.Now;
        public virtual long Id { get; set; }
        [Required]
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Institute { get; set; }
        [Required]
        //[BaseVm.ValidateDateRange]
        public virtual DateTime StartDate
        {
            get
            {
                return _returnDate;
            }
            set { _returnDate = value; }
        }
        //[BaseVm.ValidateDateRange]
        public virtual DateTime CloseDate
        {
            get
            {
                return _returnCloseDate;
            }
            set { _returnCloseDate = value; }
        }
        public virtual Guid ImageGuid { get; set; }
        public virtual byte[] Cirtificate { get; set; }
        public virtual string JobSeeker { get; set; }
    }
}