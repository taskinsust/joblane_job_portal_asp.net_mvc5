using System;
using System.Diagnostics;

namespace Model.JobLanes.Extensions
{
    public static class DateTimeExtension
    {
        [DebuggerStepThrough]
        public static bool IsValid(this DateTime target)
        {
            return (target >= DateTime.MinValue) && (target <= DateTime.MaxValue);
        }                           
    }
}