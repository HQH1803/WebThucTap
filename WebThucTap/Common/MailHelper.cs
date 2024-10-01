using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace WebThucTap.Common
{
    public class MailHelper
    {
        public void SendMail(string toEmailAddress, string subject, string content, string ccEmailAddress = null)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

            bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

            string body = content;
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            // Nếu có địa chỉ email CC, thêm vào message
            if (!string.IsNullOrEmpty(ccEmailAddress))
            {
                message.CC.Add(ccEmailAddress);
            }

            var client = new SmtpClient
            {
                EnableSsl = enabledSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword),
                Host = smtpHost,
                Port = int.Parse(smtpPort)
            };

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                // Ghi log hoặc hiển thị thông báo lỗi
                Console.WriteLine("Lỗi khi gửi email: " + ex.Message);
            }
        }
    }
}
