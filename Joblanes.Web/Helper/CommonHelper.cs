using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using JobSeekerEduVm = Web.Joblanes.Models.ViewModel.JobSeekerEduVm;

namespace Web.Joblanes.Helper
{
    public interface ICommonHelper
    {
        Dictionary<string, int> GetStatus();
        Dictionary<int, string> LoadEmumToDictionary<T>(List<int> excludeList = null);
        long GetCurrentUserProfileId();
    }

    public class CommonHelper : ICommonHelper
    {
        public Dictionary<int, string> LoadEmumToDictionary<T>(List<int> excludeList = null)
        {
            try
            {
                var enumType = typeof(T);
                Dictionary<int, string> enumToDictionary = new Dictionary<int, string>();
                var enumAllItems = Enum.GetValues(enumType);
                foreach (var currentItem in enumAllItems)
                {
                    if ((excludeList != null && !excludeList.Contains((int)currentItem)) || (excludeList == null))
                    {
                        DescriptionAttribute[] allDescAttributes = (DescriptionAttribute[])enumType.GetField(currentItem.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                        string description = allDescAttributes.Length > 0
                            ? allDescAttributes[0].Description
                            : currentItem.ToString();

                        enumToDictionary.Add((int)currentItem, description);
                    }
                }
                return enumToDictionary;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<string, int> GetStatus()
        {
            try
            {
                var status = new Dictionary<string, int>
               {
                   {"Active", EntityStatus.Active},
                   {"Inactive", EntityStatus.Inactive}
               };
                return status;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public long GetCurrentUserProfileId()
        {
            var session = HttpContext.Current.Session;
            if (session["UserProfileId"] != null)
            {
                return (long)session["UserProfileId"];
            }
            return 0;
            //UserProfileDto profileDto = _webAdminService.GetUserProfileDtoByAspId(user.Id);
        }
    }
    public static class EntityStatus
    {
        public static int Active { get { return 1; } }
        public static int Inactive { get { return -1; } }
        public static int Delete { get { return -404; } }
        public static int Pending { get { return 2; } }
        public static int Missing { get { return 0; } }
    }

    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string AdditionalValue { get; set; }
        public Response(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public Response(bool isSuccess, string message, string additionalValue)
        {
            IsSuccess = isSuccess;
            Message = message;
            AdditionalValue = additionalValue;
        }
    }

    public static class CopyClass
    {
        /// <summary>
        /// Copy an object to destination object, only matching fields will be copied
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceObject">An object with matching fields of the destination object</param>
        /// <param name="destObject">Destination object, must already be created</param>
        public static void CopyObject<T>(object sourceObject, ref T destObject)
        {
            //  If either the source, or destination is null, return
            if (sourceObject == null || destObject == null)
                return;

            //  Get the type of each object
            Type sourceType = sourceObject.GetType();
            Type targetType = destObject.GetType();

            //  Loop through the source properties
            foreach (PropertyInfo p in sourceType.GetProperties())
            {
                //  Get the matching property in the destination object
                PropertyInfo targetObj = targetType.GetProperty(p.Name);
                //  If there is none, skip
                if (targetObj == null)
                    continue;

                //  Set the value in the destination
                targetObj.SetValue(destObject, p.GetValue(sourceObject, null), null);
            }
        }
        public static void CopyObectList<T>(Array sourceObjects, T targetObjectType, ref List<T> destObjects) where T : new()
        {
            int sIndex = 1;
            int dIndex = 1;
            foreach (var sourceObject in sourceObjects)
            {
                Type sourceType = sourceObject.GetType();
                //foreach (var destObject in destObjects)
                //{
                targetObjectType = new T();
                Type targetType = targetObjectType.GetType();
                // if (sIndex == dIndex)
                //{
                foreach (PropertyInfo p in sourceType.GetProperties())
                {
                    PropertyInfo targetObj = targetType.GetProperty(p.Name);
                    //  If there is none, skip
                    if (targetObj == null)
                        continue;
                    //  Set the value in the destination
                    targetObj.SetValue(targetObjectType, p.GetValue(sourceObject, null), null);

                    //break;
                }
                destObjects.Add(targetObjectType);
                //}
                //dIndex++;
                //}
                //sIndex++;
            }
        }
    }

    public static class JavaScriptSerializerExtensions
    {
        public static dynamic DeserializeDynamic(this JavaScriptSerializer serializer, string value)
        {
            var dictionary = serializer.Deserialize<IDictionary<string, object>>(value);
            return GetExpando(dictionary);
        }

        private static ExpandoObject GetExpando(IDictionary<string, object> dictionary)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();

            foreach (var item in dictionary)
            {
                var innerDictionary = item.Value as IDictionary<string, object>;
                if (innerDictionary != null)
                {
                    expando.Add(item.Key, GetExpando(innerDictionary));
                }
                else
                {
                    expando.Add(item.Key, item.Value);
                }
            }

            return (ExpandoObject)expando;
        }
    }
    //public  class CustomCountry 
    //{
    //    public List<CustomCountry> getCountry()
    //    {
           
    //        return new List<CustomCountry>();
    //    }
    //}
}