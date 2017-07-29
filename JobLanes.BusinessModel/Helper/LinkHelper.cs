using System.Web;
using Model.JobLanes.Extensions;

namespace Model.JobLanes.Helper
{
    public class LinkHelper
    {
        public static string Domain
        {
            get
            {
                string domain = string.Empty;

                string applicationPath = CurrentRequest.ApplicationPath;
                if (applicationPath == "/") applicationPath = string.Empty;

                string requestPath = CurrentRequest.Url.AbsolutePath.Substring(applicationPath.Length);
                string host = CurrentRequest.Url.Host;
                string port = CurrentRequest.Url.Port.ToString();
                string protocol = CurrentRequest.IsSecureConnection ? "https" : "http";

                if (CurrentRequest.Url.IsDefaultPort)
                    domain = "{0}://".FormatWith(protocol) + host + applicationPath;
                else
                    domain = "{0}://".FormatWith(protocol) + host + ":" + port + applicationPath;

                return domain;
            }
        }

        public static string DomainWithoutProtocol
        {
            get
            {
                string domain = string.Empty;

                string applicationPath = CurrentRequest.ApplicationPath;
                if (applicationPath == "/") applicationPath = string.Empty;

                string requestPath = CurrentRequest.Url.AbsolutePath.Substring(applicationPath.Length);
                string host = CurrentRequest.Url.Host;
                string port = CurrentRequest.Url.Port.ToString();

                if (CurrentRequest.Url.IsDefaultPort)
                    domain = host + applicationPath;
                else
                    domain = host + ":" + port + applicationPath;

                return domain;
            }
        }

        private static HttpRequest CurrentRequest
        {
            get
            {
                var current = HttpContext.Current;
                Check.Argument.Null(current, "httpcontext", "No httpcontext is available");

                var request = current.Request;
                Check.Argument.Null(request, "request", "No request available on current httpcontext.");

                return request;
            }
        }

    }
}
    