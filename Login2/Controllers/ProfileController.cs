using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Login2.Models;

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
                profile.AddProfToDB(profile);

            }
            
            return View(bigModel);
        }

        [HttpGet]
        public ActionResult ProfileView ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProfileView (Models.Profile profile)
        {
            return View(profile);
        }
    }
}