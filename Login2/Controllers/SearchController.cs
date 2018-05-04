using System.Web.Mvc;
using Login2.Controllers;
using System.Data.SqlClient;
using Login2.Models;
using System.Web;
using System.Collections.Generic;
using System;

namespace Login2.Controllers
{
    public class SearchController : Controller
    {
        ProfileController profcontroller = new ProfileController();

        // GET: Search

        //Make viewbag return a list of profiles.
        public ActionResult SearchProfile(string SearchText)
        {
            var ProfList = new List<Profile>();
            var LikeList = new List<int>();
            string[] usernames = GetUsers(SearchText);
            

            if (usernames != null)
            {
                for (int i = 0; i < usernames.Length; i++)
                {
                    ProfList.Add(profcontroller.ReturnProfile(usernames[i]));
                    int LikeCOunter = LikeCount(usernames[i]);
                    System.Diagnostics.Debug.WriteLine(LikeCOunter);
                    LikeList.Add(LikeCOunter);
                }
                ViewBag.Message = ProfList;
            }
            else
            {
                ViewBag.Message = null;
            }
            ViewBag.ListCount = LikeList ;

            return View();
        }

        private string[] GetUsers(string Username)
        {
            SqlDataReader reader = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            var constants = new Constants();
            List<string> listNames = new List<string>();

            cn = new SqlConnection(Constants.ConnString);
            string _sql = (@"SELECT username FROM Users WHERE username LIKE @username");
            string WildUsername = string.Format("{0}%", Username);
            cmd = new SqlCommand(_sql, cn);
            cmd.Parameters.AddWithValue("@username", WildUsername);
            System.Diagnostics.Debug.WriteLine(string.Format("Search for {0}", Username));
            cn.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read()) 
                {
                    listNames.Add(reader["username"].ToString());
                    System.Diagnostics.Debug.WriteLine("Found " +  reader["username"].ToString() + " in database");
                }
                return (listNames.ToArray());
            }
            else
            {
                return (null);
            }
        }

        public int LikeCount(string Username)
        {
            System.Diagnostics.Debug.WriteLine("Inside LikeCount");

            SqlDataReader reader = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;

            int LikeCounter = 0;

            try
            {
                cn = new SqlConnection(Constants.ConnString);
                string _sql = @"SELECT COUNT(*) AS LikeCount FROM LIKES WHERE profile = @profile";
                cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@profile", Username);

                cn.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                LikeCounter = (int)reader["LikeCount"];
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
           
            return (LikeCounter);
        }
    }
}