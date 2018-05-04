using System;
using System.Data.SqlClient;
using System.Net.Mail;
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
                    if (Session["Usertoken"] != null)
                    {
                        user = user.getUserInfo(user.Username);

                        string body = "Please follow this link to verify your account http://" +
                                        Request.Url.Authority + "/User/VerifyAccount?token=" + 
                                        user.Token + "&username=" + user.Username;
                        string subject = "Matcha Registration";

                        var sendMail = new Models.SendMail.SendMail();
                        bool sent = sendMail.SendEmail(user.Email, subject, body);

                        if (sent)
                        {
                            TempData["user"] = user;
                            return RedirectToAction("EmailSent", "User");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
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
                    string subject = "Matcha Registration";

                    System.Diagnostics.Debug.WriteLine(user.Token);
                    string body = "Please follow this link to verify your account http://" +
                        Request.Url.Authority + "/User/VerifyAccount?token=" + user.Token + 
                        "&username=" + user.Username;

                    var sendMail = new Models.SendMail.SendMail();
                    bool sent = sendMail.SendEmail(user.Email, subject, body);

                    if (sent) {
                        TempData["user"] = user;
                        return RedirectToAction("EmailSent", "User");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
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
            HomeController HC = new HomeController();
            HC.ConnectionStatus(false, Session["Username"].ToString());
            FormsAuthentication.SignOut();
            if (Session != null)
                Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult VerifyAccount (string token, string username)
        {
            SqlConnection cn = null;
            SqlCommand cmd = null;

            try
            {
                cn = new SqlConnection(Constants.ConnString);
                string _sql = @"UPDATE Users SET token = 0 WHERE username = @username AND token = @token";
                cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@token", token);
                cn.Open();
                cmd.ExecuteReader();
                cn.Close();
                System.Diagnostics.Debug.WriteLine("Validated account for " + username);
                return View();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return View();
            }

        }
    }
}