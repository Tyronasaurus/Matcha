using System;
using System.Net;
using System.Net.Mail;


namespace Login2.Models.SendMail
{
    public class SendMail
    {
        public bool SendEmail(string receiver, string subject, string message)
        {
            try
            {
                var senderEmail = new MailAddress("tbarlow-@student.wethinkcode.co.za", "Tyron From Matcha");
                var receiverEmail = new MailAddress(receiver, "Receiver");
                var password = "WvdSu9yw";
                var sub = subject;
                var body = message;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
                System.Diagnostics.Debug.WriteLine("Mail sent");
                return (true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}