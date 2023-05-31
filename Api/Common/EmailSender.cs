using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Net.Mail;

namespace Api.Common
{
    public class EmailSender : IEmailSender
    {
        //private readonly SmtpSettings _smtpSettings;

        //public EmailSender(SmtpSettings smtpSettings)
        //{
        //    _smtpSettings = smtpSettings;
        //}

        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress("abolfazl.barzegar77@hotmail.com");
            MailMessage emailMess = new MailMessage(from, to);
            emailMess.Subject = subject;
            emailMess.Body = message;
            emailMess.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = "smtp-mail.outlook.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("abolfazl.barzegar77@hotmail.com", "Abolfazl1377!");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            try
            {
                /* Send method called below is what will send off our email 
                 * unless an exception is thrown. */
                smtp.Send(emailMess);
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
        }
    }


    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string subject, string message);
    }

    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
    }
}
