using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Helper
{
    public class Constants
    {

    }
    public static class JobNature
    {
        public const int PartTime = 1;
        public const int Temporary = 2;
        public const int Commision = 3;
        public const int Internship = 4;
        public const int FullTime = 5;
        public const int Contract = 6;
    }
    public static class SalaryDuration
    {
        public static int Hourly = 1;
        public static int Daily = 2;
        public static int Weekly = 3;
        public static int Monthly = 4;
        public static int Yearly = 5;

        public static Dictionary<string, int> GetSalaryDuration()
        {
            try
            {
                var status = new Dictionary<string, int>
               {
                   {"Hourly",SalaryDuration.Hourly},
                   {"Daily", SalaryDuration.Daily},
                   {"Weekly",SalaryDuration.Weekly},
                   {"Monthly", SalaryDuration.Monthly},
                   {"Yearly",SalaryDuration.Yearly}
                
               };
                return status;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }

    public static class InLocation
    {
        public static int InsideUsa = 1;
        public static int OutSideUsa = 2;
    }
    public static class Eligibility
    {
        public static int AuthorizedToWorkInUsa = 1;
        public static int UnAuthorizedToWorkInUsa = 2;
    }

    public static class UserRole
    {
        public static string JobSeeker = "Job seekers";
        public static string WebAdmin = "Web Admin";
        public static string Company = "Employers";
    }
}