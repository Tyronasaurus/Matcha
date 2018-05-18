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
            Like likeModel = new Like();
            Profile profile = new Profile();
            System.Diagnostics.Debug.WriteLine("Inside Like Post " + liked_Profile );
            SqlDataReader reader = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _sql = null;
            bool LikeStat = true;

            if (Session["Username"] == null)
            {
                var UC = new UserController();
                UC.Logout();
            }

            string liked_User = Session["Username"].ToString();
            try
            {
                cn = new SqlConnection(Constants.ConnString);
                LikeStat = likeModel.LikeUnlike(liked_Profile, liked_User);


                if (LikeStat == true)
                {
                    //LIKES
                    cn.Close();
                    _sql = @"INSERT INTO Likes (liked_user, profile_liked) VALUES (@liked_user, @profile)";
                    cmd = new SqlCommand(_sql, cn);
                    cmd.Parameters.AddWithValue("@liked_user", liked_User);
                    cmd.Parameters.AddWithValue("@profile", liked_Profile);
                    cn.Open();
                    reader = cmd.ExecuteReader();
                    SendMailLike(liked_Profile, liked_User);
                }
                else
                {
                    //UNLIKES
                    cn.Close();
                    _sql = @"DELETE FROM Likes WHERE liked_user = @liked_user AND profile_liked = @profile";
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
            System.Diagnostics.Debug.WriteLine(liked_User + " " + LikeStat + " " + liked_Profile);

            return RedirectToAction("SingleProfileView", "Profile", new { Username = liked_Profile });
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