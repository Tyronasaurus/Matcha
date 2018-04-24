using Login2.Models;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Login2.Controllers
{
    public class LikeController : Controller
    {
        // GET: Like
        public ActionResult LikeClicked(string ProfileName)
        {
            return RedirectToAction("SearchProfile", "Search");
        }

        [HttpPost]
        public ActionResult LikeClicked ()
        {
            SqlDataReader reader = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;

            string liked_Profile = Request["HiddenName"];
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
                    _sql = @"INSERT INTo Likes (liked_user, profile) VALUES (@liked_user, @profile)";
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
            System.Diagnostics.Debug.WriteLine(liked_User + " likes " + liked_Profile);
            return RedirectToAction("SearchProfile", "Search");
        }
    }
}   