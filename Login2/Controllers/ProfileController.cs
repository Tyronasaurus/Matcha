using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Login2.Models;
using System.Data;

namespace Login2.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        [HttpGet]
        public ActionResult ProfileEditor()
        {

            //------TAGS-----\\

            List<ProfileTags> list = new List<ProfileTags>();
            using (var cn = new SqlConnection(Constants.ConnString))
            {
                string _sql = @"SELECT * FROM [dbo].[Tags]";
                var cmd = new SqlCommand(_sql, cn);
                cn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProfileTags t = new ProfileTags
                    {
                        TagID = (int)reader["Id"],
                        TagName = (string)reader["TagName"]
                    };
                    list.Add(t);
                }

                cn.Close();
            }

            var DBModel = new ProfileTagsList()
            {
                DBTags = list
            };

            var BigModel = new BigModel
            {
                ProfileTagsList = DBModel
            };
            //-----END TAGS-----\\

            return View(BigModel);
        }

        [HttpPost]
        public ActionResult ProfileEditor(BigModel bigModel)
        {
            if (ModelState.IsValid)
            {
                var profile = bigModel.Profile;
                var profileTags = new List<ProfileTags>();
                var DBTags = bigModel.ProfileTagsList.DBTags;

                
                for (int i = 0; i < DBTags.Count; i++)
                {
                    if (DBTags[i].IsSelected)
                    {
                        profileTags.Add(DBTags[i]);
                    }
                }
                profile.Tags = profileTags;
                bool Added = profile.AddProfToDB(profile);
                System.Diagnostics.Debug.WriteLine("Added to db: " + Added);
                if (Added == true)
                {
                    return RedirectToAction("ProfileView", "Profile");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            
            return View(bigModel);
        }

        [HttpGet]
        public ActionResult ProfileView ()
        {
            SqlDataReader reader = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
 

            if (Session["id"] == null)
            {
                if (Session["Username"] != null)
                {
                    try
                    {
                        cn = new SqlConnection(Constants.ConnString);
                        cmd = new SqlCommand("SELECT id FROM [dbo].[Users] WHERE username = @user", cn);
                        cmd.Parameters.AddWithValue("@user", Session["Username"]);
                        cn.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                    finally
                    {
                        Session["id"] = reader["id"];
                        cn.Close();
                    }
                }
                else
                {
                    var UC = new UserController();
                    UC.Logout();
                }
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("Inside try");
                cn = new SqlConnection(Constants.ConnString);
                string _sql = @"SELECT * FROM [dbo].[Profile] WHERE userid = @userid";
                cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@userid", Session["id"]);
                cn.Open();
                reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    var prof = new Profile();
                    prof.Tags = new List<ProfileTags>();
                    System.Diagnostics.Debug.WriteLine(reader["gender"]);
                    if (reader["gender"] != null)
                        prof.Gender = reader["gender"].ToString();
                    if (reader["sexPref"] != null)
                        prof.SexPref = reader["sexPref"].ToString();
                    if (reader["bio"] != null)
                        prof.Bio = reader["bio"].ToString(); 
                    if (reader["tags"] != null)
                    {
                        var TagString = reader["tags"].ToString().Split(',');
                        System.Diagnostics.Debug.WriteLine(TagString.Length.ToString());
                        ProfileTags newtag = new ProfileTags();
                        for (int i = 0; i < TagString.Length; i++)
                        {
                            newtag = new ProfileTags();

                            newtag.TagName = TagString[i].ToString();
                            
                            prof.Tags.Add(newtag);
                        }
                    }
                    ViewBag.Message = prof;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Did not set Viewbag");
                    return RedirectToAction("ProfileEditor", "Profile");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR " + ex);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ProfileView (Models.Profile profile)
        {

            return View(profile);
        }
    }
}