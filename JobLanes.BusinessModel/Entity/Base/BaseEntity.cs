using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.JobLanes.Entity.Base
{
    public interface IBaseEntity<TTdT>
    {
        TTdT Id { get; set; }
        int VersionNumber { get; set; }
        string BusinessId { get; set; }
        DateTime CreationDate { get; set; }
        DateTime ModificationDate { get; set; }
        int Status { get; set; }
        string Name { get; set; }
        long CreateBy { get; set; }
        string CreateByText { get; set; }
        long ModifyBy { get; set; }
        string ModifyByText { get; set; }
    }
    public class BaseEntity<TTdT> : IBaseEntity<TTdT>, ICloneable
    {
        public BaseEntity()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }
        public virtual TTdT Id { get; set; }
        public virtual int VersionNumber { get; set; }
        public virtual string BusinessId { get; set; }

        [DisplayName("Creation Date")]
        public virtual DateTime CreationDate { get; set; }
        [DisplayName("Modification Date")]
        public virtual DateTime ModificationDate { get; set; }
        public virtual int Status { get; set; }

        //[Required(ErrorMessage = "Name is required.")]
        public virtual string Name { get; set; }
        [DisplayName("Create By")]
        public virtual long CreateBy { get; set; }
        [DisplayName("Create By")]
        public virtual string CreateByText { get; set; }

        [DisplayName("Modify By")]
        public virtual long ModifyBy { get; set; }
        [DisplayName("Modify By")]
        public virtual string ModifyByText { get; set; }


        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            return "";
        }
        public static class EntityStatus
        {
            public static int Active { get { return 1; } }
            public static int Inactive { get { return -1; } }
            public static int Delete { get { return -404; } }
            public static int Pending { get { return 2; } }
            public static int Missing { get { return 0; } }
        }


        public static class EditableStatus
        {
            public static bool IsEditable { get { return true; } }
            public static bool NotEditable { get { return false; } }

        }


        object ICloneable.Clone()
        {
            // simply delegate to our type-safe cousin
            return this.Clone();
        }

        public virtual object Clone()
        {
            // Start with a flat, member-wise copy
            return this.MemberwiseClone();
        }
    }

    public static class StatusTypeText
    {
        public static string Active { get { return "Active"; } }
        public static string Inactive { get { return "Inactive"; } }
        public static string Deleted { get { return "Deleted"; } }
        public static string Pending { get { return "Pending"; } }

        public static string GetStatusText(int status)
        {
            if (status == BaseEntity<long>.EntityStatus.Active)
            {
                return Active;
            }
            else if (status == BaseEntity<long>.EntityStatus.Inactive)
            {
                return Inactive;
            }
            else if (status == BaseEntity<long>.EntityStatus.Delete)
            {
                return Deleted;
            }
            else if (status == BaseEntity<long>.EntityStatus.Pending)
            {
                return Pending;
            }
            else
            {
                return "Unknown";
            }
        }
    }

    public class ValidateDateRange : ValidationAttribute
    {
        public ValidationResult IsValid(DateTime value, ValidationContext validationContext)
        {
            // your validation logic
            if (value >= Convert.ToDateTime("01/10/1920") && value <= DateTime.MaxValue)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date is not in given range.");
            }
        }
    }
}
