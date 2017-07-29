using System;
using System.Diagnostics;
using Model.JobLanes.Helper;

namespace Model.JobLanes.Extensions
{
    public static class GuidExtension
    {
        [DebuggerStepThrough]
        public static string Shrink(this Guid target)
        {
            Check.Argument.Empty(target, "target");

            string base64 = Convert.ToBase64String(target.ToByteArray());

            string encoded = base64.Replace("/", "_").Replace("+", "-");

            return encoded.Substring(0, 22);
        }

        public static string NewGuidString(this Guid target)
        {
            Check.Argument.Empty(target, "target");

            return Guid.NewGuid().ToString().Replace("-", "");
        }

        [DebuggerStepThrough]
        public static bool IsEmpty(this Guid target)
        {
            return target == Guid.Empty;
        }
    }
}