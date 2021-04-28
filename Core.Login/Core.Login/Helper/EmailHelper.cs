using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Core.Login.Helper
{
    public class EmailHelper
    {
        private static string fromaAddress { get; } = "email address";
        private static string password { get; } = "password";

        public static void Send(string title, string messageText, string sendMail)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(fromaAddress);
                message.To.Add(new MailAddress(sendMail));
                message.Subject = title;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = messageText;

                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromaAddress, password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }
    }
}
