using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Login2.Models;


namespace Login2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        public ActionResult EmailSent()
        {
            User user = (User)TempData["user"];
            ViewBag.Message = user;

            return View();
        }

        [HttpGet]
        public ActionResult Login ()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmailSent (User user)
        {
            return View(user);
        }

        [HttpPost]
        public ActionResult Login(Models.User user)
        {
            
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.Username, user.Password))
                {
                    Session["LoggedIn"] = true;
                    Session["Username"] = user.Username;
                    Session["Token"] = user.Token;
                    FormsAuthentication.SetAuthCookie(user.Username, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to login. Please try again");
                }
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Registration(user))
                {
                    SendMail(user);

                    TempData["user"] = user;
                    return RedirectToAction("EmailSent", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to register. Please try again");
                }
            }
            return (View(user));
        }

        public ActionResult Logout ()
        {
            FormsAuthentication.SignOut();
            if (Session != null)
                Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public void SendMail(Models.User user)
        {

            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress("tbkearsley@gmail.com", "Matcha Registration");
                    mailMessage.From = fromAddress;
                    mailMessage.To.Add(user.Email);
                    mailMessage.Body = "This is Testing Email Without Configured SMTP Server";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = " Testing Email";
                    mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "localhost";
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}