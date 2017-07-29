using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Models
{
    public class Contants
    {
        public static int FirstBussinessYear { get { return 2016; } }
        public static int MessageSuccess { get { return 0; } }
        public static int MessageError { get { return 1; } }
        public static int PageSize { get { return 10; } }
        public static DateTime MaxDateTime { get { return DateTime.MaxValue; } }
        public static DateTime MinDateTime { get { return new DateTime(1950, 1, 1); } }
    }
}