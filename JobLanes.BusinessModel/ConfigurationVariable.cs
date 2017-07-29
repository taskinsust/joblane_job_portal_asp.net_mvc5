using System;
using System.Configuration;
using Model.JobLanes.Extensions;
using Model.JobLanes.Helper;

namespace Model.JobLanes
{
    public class ConfigurationVariable<T>
    {
        private readonly string _key;

        public ConfigurationVariable(string key)
        {
            Check.Argument.EmptyOrWhiteSpace(key, "key");
            _key = key;
        }

        private object GetInternalValue()
        {
            string value = ConfigurationManager.AppSettings.Get(_key);
            Check.Argument.EmptyOrWhiteSpace(value, "appsetting", "AppSetting: {0} is not configured.".FormatWith(_key));

            try
            {
                if (typeof(T).Equals(typeof(int))) return int.Parse(value);
                if (typeof(T).Equals(typeof(string))) return value;
                if (typeof(T).Equals(typeof(bool))) return Convert.ToBoolean(value);
                if (typeof(T).Equals(typeof(double))) return Convert.ToDouble(value);

                throw new Exception("Type not supported.");
            }
            catch (Exception ex)
            {
                throw new Exception("Config key:{0} was expected to be of type {1} but was not.".FormatWith(_key, typeof(T)), ex);
            }
        }

        public T Value
        {
            get
            {
                var v = GetInternalValue();
                Check.Argument.Null(v, "key", "The configuratoin file does not contain any value for '{0}'.".FormatWith(_key));

                return (T)v;
            }
        }
    }
}
