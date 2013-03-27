using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Common
{
    public class Email
    {
        public bool SendEmail(string content,string title,string server,string from,string to,int port,string pwd)
        {
            System.Net.Mail.SmtpClient client = new SmtpClient(server);
            client.Timeout = 500000;
            client.UseDefaultCredentials = false;
            client.Port = port;
            client.Credentials = new System.Net.NetworkCredential(from, pwd);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                if (to.IndexOf(";") < 0)
                {
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to, title, content);
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.IsBodyHtml = true;
                    client.Send(message);
                }
                else
                {
                    string[] stra = to.Split(new char[] { ';' });
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, stra[0].Trim(), title, content);
                    for (int i = 1; i < stra.Length; i++)
                    {
                        if (stra[i].Trim() != "")
                        {
                            message.To.Add(stra[i].Trim());
                        }

                    }
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.IsBodyHtml = true;
                    client.Send(message);

                }
                return true;
            }
            catch
            {
                return false;
            }
         
        }
    }
}
