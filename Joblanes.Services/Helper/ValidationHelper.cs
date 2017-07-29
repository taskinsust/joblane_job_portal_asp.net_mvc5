using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Util;
using System.ComponentModel.DataAnnotations;
using Services.Joblanes.Exceptions;
namespace Services.Joblanes.Helper
{
    public class EntityValidationResult
    {

        public IList<ValidationResult> Errors { get; private set; }

        public bool HasError
        {
            get { return Errors.Count > 0; }
        }

        public EntityValidationResult(IList<ValidationResult> errors = null)
        {
            Errors = errors ?? new List<ValidationResult>();
        }

    }

    public class EntityValidator<TEntityT, TMDataM>
    {
        public EntityValidationResult Validate(TEntityT entity)
        {
            if (typeof(TEntityT) != typeof(TMDataM))
            {
                TypeDescriptor.AddProviderTransparent(
                    new AssociatedMetadataTypeTypeDescriptionProvider(typeof(TEntityT), typeof(TMDataM)), typeof(TEntityT));
            }
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);
            return new EntityValidationResult(validationResults);
        }
    }

    public class ValidationHelper
    {
        public static EntityValidationResult ValidateEntity<TEntityT, TMDataM>(TEntityT entity)
        {
            return new EntityValidator<TEntityT, TMDataM>().Validate(entity);
        }
    }

    public class CustomModelValidationCheck
    {
        public static void DataAnnotationCheck<T, TMetaData>(T tObj, TMetaData tMetaData)
        {
            var validationResult = ValidationHelper.ValidateEntity<T, TMetaData>(tObj);
            if (validationResult.HasError)
            {
                string errorMessage = "";
                validationResult.Errors.ForEach(r => errorMessage = errorMessage + r.ErrorMessage + Environment.NewLine);
                throw new MessageException(errorMessage);
            }
        }
    }
}
