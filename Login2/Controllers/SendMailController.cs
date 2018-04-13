using Login2.Models.SendMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;

namespace Login2.Controllers
{
    public class SendMailController : ApiController
    {

        [HttpPost]
        [ActionName("sendmail")]
        public IHttpActionResult ProcessAuthEmail(SendMail mailModel)
        {
            MailMessage msg = new MailMessage();

            msg.To.Add(new MailAddress(mailModel.Recipient));
            msg.From = new MailAddress("matcha@gmail.com", "Matcha Registration");
            msg.Subject = mailModel.Subject;
            msg.Body = mailModel.Body;

            SmtpClient client = new SmtpClient("smtp.google.com");
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            NetworkCredential cred = new System.Net.NetworkCredential("tbkearsley@gmail.com", "samwisegamgee");
            client.Credentials = cred;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                client.Send(msg);
                msg.Dispose();
                return Ok("Mail sent");
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                return (NotFound());
            }
        }
    }
}
