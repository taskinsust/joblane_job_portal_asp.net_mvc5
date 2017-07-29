using System;
using System.Collections.Generic;
using System.Net.Mail;
using Model.JobLanes.Extensions;

namespace Model.JobLanes
{
    public class EmailSender
    {
        public static void Send(string receiver, string subject, string body)
        {
            Send(new KeyValuePair<string, string>(receiver, string.Empty), subject, body);
        }
        public static void Send(string receiver, string subject, string body, List<string> attachments)
        {
            Send(new KeyValuePair<string, string>(receiver, string.Empty), subject, body, attachments);
        }


        public static void Send(KeyValuePair<string, string> receiver, string subject, string body)
        {
            Send(new Dictionary<string, string> { {receiver.Key, receiver.Value} }, subject, body);
        }
        public static void Send(KeyValuePair<string, string> receiver, string subject, string body, List<string> attachments)
        {
            Send(new Dictionary<string, string> { {receiver.Key, receiver.Value} }, subject, body, attachments);
        }


        public static void Send(Dictionary<string, string> receivers, string subject, string body)
        {
            Send(receivers, subject, body, null, null, null, true);
        }
        public static void Send(Dictionary<string, string> receivers, string subject, string body, List<string> attachments)
        {
            Send(receivers, subject, body, attachments, null, null, true);
        }


        public static void Send(Dictionary<string, string> receivers, string subject, string body, List<string> attachments, Dictionary<string, string> ccList, Dictionary<string, string> bccList, bool isHtml)
        {
            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(App.Configurations.FromEmail.Value, App.Configurations.FromEmailName.Value);

            var eReceivers = receivers.GetEnumerator();
            while (eReceivers.MoveNext())
            {
                mailMessage.To.Add(eReceivers.Current.Value.IsNullOrEmpty()
                                       ? new MailAddress(eReceivers.Current.Key)
                                       : new MailAddress(eReceivers.Current.Key, eReceivers.Current.Value));
            }

            if (ccList != null)
            {
                var eCcList = ccList.GetEnumerator();
                while (eCcList.MoveNext())
                {
                    mailMessage.CC.Add(eCcList.Current.Value.IsNullOrEmpty()
                                           ? new MailAddress(eCcList.Current.Key)
                                           : new MailAddress(eCcList.Current.Key, eCcList.Current.Value));
                }
            }

            if (bccList != null)
            {
                var eBccList = bccList.GetEnumerator();
                while (eBccList.MoveNext())
                {
                    mailMessage.Bcc.Add(eBccList.Current.Value.IsNullOrEmpty()
                                            ? new MailAddress(eBccList.Current.Key)
                                            : new MailAddress(eBccList.Current.Key, eBccList.Current.Value));
                }
            }

            if (attachments != null)
            {
                foreach (string attachment in attachments)
                {
                    mailMessage.Attachments.Add(new Attachment(attachment));
                }
            }

            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = isHtml;

            try
            {
                var smtp = new SmtpClient();                
                
                smtp.Send(mailMessage);
                smtp = null;

                GC.Collect();
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;                
            }

            mailMessage.Dispose();
        }

    }
}
