using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Model.JobLanes;
using Model.JobLanes.Helper;

namespace Services.Joblanes
{
    public interface IEmailServiceNew
    {
        string TemplateFolder { get; set; }
        void SendRegisterConfirmationEmail(string mail, string receptName, string code);
    }

    public class EmailServiceNew : IEmailServiceNew
    {

        public EmailServiceNew()
        {
            if (HttpContext.Current != null)
            {
                var ss = App.Configurations.EmailTemplatePath.Value;
                string path = HttpContext.Current.Server.MapPath(App.Configurations.EmailTemplatePath.Value);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                TemplateFolder = path;
            }
        }

        public string TemplateFolder { get; set; }
        public void SendRegisterConfirmationEmail(string receiverEmail, string receptName, string code)
        {
            if (String.IsNullOrEmpty(TemplateFolder)) return;

            string body = System.IO.File.ReadAllText(TemplateFolder + "\\" + App.Configurations.RegistrationEmailName.Value);
            string subject = App.Configurations.RegistrationEmailSubject.Value;


            body = body.Replace("$ReceiverName$", receptName);
            body = body.Replace("$Logo$", LinkHelper.Domain + "~/Content/assets/images/logo.png");
            body = body.Replace("$Recipient$", receiverEmail);
            body = body.Replace("$code$", code);

            EmailSender.Send(receiverEmail, subject, body);
        }
    }
}
