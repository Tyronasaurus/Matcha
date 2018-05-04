using Login2.Models;
using Login2.Models.SendMail;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Login2.Controllers
{
    public class LikeController : Controller
    {
        public ActionResult GetLikeCount ()
        {
            
            return RedirectToAction("SearchProfile", "Search");
        }

        // GET: Like
        public ActionResult LikeClicked()
        {
            System.Diagnostics.Debug.WriteLine("Inside Like Get");
            return RedirectToAction("SingleProfileView", "Profile");
        }

        [HttpPost]
        public ActionResult LikeClicked (string liked_Profile)
        {
            System.Diagnostics.Debug.WriteLine("Inside Like Post");
            SqlDataReader reader = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;

            if (Session["Username"] == null)
            {
                var UC = new UserController();
                UC.Logout();
            }

            string liked_User = Session["Username"].ToString();
            try
            {
                cn = new SqlConnection(Constants.ConnString);
                string _sql = @"SELECT * FROM Likes WHERE liked_user = @liked_user AND profile = @profile";
                cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@liked_user", liked_User);
                cmd.Parameters.AddWithValue("@profile", liked_Profile);
                cn.Open();
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    cn.Close();
                    _sql = @"INSERT INTO Likes (liked_user, profile) VALUES (@liked_user, @profile)";
                    cmd = new SqlCommand(_sql, cn);
                    cmd.Parameters.AddWithValue("@liked_user", liked_User);
                    cmd.Parameters.AddWithValue("@profile", liked_Profile);
                    cn.Open();
                    reader = cmd.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            SendMailLike(liked_Profile, liked_User);
            System.Diagnostics.Debug.WriteLine(liked_User + " likes " + liked_Profile);
            return Redirect(Request.Url.Authority + "?Username=" + liked_Profile);
        }


        public void SendMailLike(string username, string liked_User)
        {
            User user = new User();
            user = user.getUserInfo(username);
            SendMail sendMail = new SendMail();

            string subject = "A potential match is in the making";
            string body = "Dear " + username + ", " + liked_User + 
                " is interested and has liked your profile! Like them back to start chatting";

            bool sent = sendMail.SendEmail(user.Email, subject, body);

            if (sent)
            {
                System.Diagnostics.Debug.WriteLine("Like mail sent");
            }


        }
    }
}   