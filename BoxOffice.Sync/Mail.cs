using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;

namespace MastiTickets.Win.Sync
{
    public class Mail
    {
        public static void SendEmailLog(string Log)
        {
            try
            {
                int TheaterID = Convert.ToInt32(ConfigurationManager.AppSettings["TheaterID"]);
                string TheaterName = ConfigurationManager.AppSettings["TheaterName"];

                SmtpClient mySmtpClient = new SmtpClient("smtp.gmail.com");

                // set smtp-client with basicAuthentication
                mySmtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new
                   System.Net.NetworkCredential("sivaram.boppana@nuvolasmart.com", "test*abc");
                mySmtpClient.Credentials = basicAuthenticationInfo;
                mySmtpClient.Port = 465;

                // add from,to mailaddresses
                MailAddress from = new MailAddress(TheaterName+ "@mastitickets.in", TheaterName);
                MailAddress to = new MailAddress("sivaram.boppana@nuvolasmart.com", "siva ram");
                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                // add ReplyTo
                //MailAddress replyto = new MailAddress("reply@example.com");
                //myMail.ReplyTo = replyto;

                // set subject and encoding
                myMail.Subject = "Error Log " + DateTime.Now;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                myMail.Body = Log;
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                // text or html
                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
            }

            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
