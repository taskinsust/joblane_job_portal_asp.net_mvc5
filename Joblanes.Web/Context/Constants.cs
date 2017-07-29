using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Web.Joblanes.Context
{
    public class Constants
    {
        public enum Gender
        {
            Male = 1,
            Female = 2,
            Combined = 3
        }
        public enum MaritalType : int
        {
            Single = 1,
            Married = 2,
            Widow = 3,
            Widower = 4,
            Divorced = 5
        }
        public enum Religion : int
        {
            Islam = 1,
            Hinduism = 2,
            Christianity = 3,
            Buddhism = 4,
            Other = 5
        }
        public enum EmployeeSize
        {
            [Description("Bellow 10")]
            Bten = 1,
            [Description("10-50")]
            TtoF = 2,
            [Description("51-100")]
            FtoH = 3,
            [Description("100-500")]
            HtoF = 4,
            [Description("500-1000")]
            FtoT = 5,
            [Description("Above 1000")]
            At = 6
        }
        public enum BloodGroup
        {
            [Description("A+")]
            Apos = 1,
            [Description("A-")]
            Aneg = 2,
            [Description("B+")]
            Bpos = 3,
            [Description("B-")]
            Bneg = 4,
            [Description("AB+")]
            ABpos = 5,
            [Description("AB-")]
            ABneg = 6,
            [Description("O+")]
            Opos = 7,
            [Description("O-")]
            Oneg = 8,
        }
        public enum JobLevel
        {
            [Description("Entry Level Job")]
            Elvl = 1,
            [Description("Mid/Managerial Level Job")]
            Mlvl = 2,
            [Description("Top Level Job")]
            Tlvl = 3,
        }
        public enum JobType
        {
            [Description("Full Time")]
            Ftype = 1,
            [Description("Part Time")]
            Ptype = 2,
            [Description("Contractual")]
            Ctype = 3,
        }
        public enum GenderJob
        {
            [Description("Male Only")]
            Male = 1,
            [Description("Female Only")]
            Female = 2,
            [Description("All")]
            All = 3
        }
        public static class EnumExtension
        {
            public static string GetDescription(Enum value)
            {
                Type type = value.GetType();
                string name = Enum.GetName(type, value);
                if (name != null)
                {
                    FieldInfo field = type.GetField(name);
                    if (field != null)
                    {
                        DescriptionAttribute attr =
                               Attribute.GetCustomAttribute(field,
                                 typeof(DescriptionAttribute)) as DescriptionAttribute;
                        if (attr != null)
                        {
                            return attr.Description;
                        }
                    }
                }
                return null;
            }
        }
        public static class Utility
        {
            public static string GetDescriptionFromEnumValue(Enum value)
            {
                DescriptionAttribute attribute = value.GetType()
                    .GetField(value.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault() as DescriptionAttribute;
                return attribute == null ? value.ToString() : attribute.Description;
            }

            public static T GetEnumValueFromDescription<T>(string description)
            {
                var type = typeof(T);
                if (!type.IsEnum)
                    throw new ArgumentException();
                FieldInfo[] fields = type.GetFields();
                var field = fields
                    .SelectMany(f => f.GetCustomAttributes(
                        typeof(DescriptionAttribute), false), (
                            f, a) => new { Field = f, Att = a }).SingleOrDefault(a => ((DescriptionAttribute)a.Att)
                                .Description == description);
                return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
            }
        }
    }
}