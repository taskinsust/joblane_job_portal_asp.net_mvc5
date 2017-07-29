using System.Web;

namespace Model.JobLanes.Extensions
{   
    public static class HttpPostedFileBaseExtension
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }
}
