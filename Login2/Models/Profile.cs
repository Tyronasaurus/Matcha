using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Net.Mail;
using System.Web.Security;
using Login2.Models;

namespace Login2.Models
{
    public class BigModel
    {
        public Profile Profile { get; set; }
        public ProfileTagsList ProfileTagsList { get; set; }
        
    }


    public class Profile
    {
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Sexual Preference")]
        public string SexPref { get; set; }

        [Display(Name = "Biography")]
        public string Bio { get; set; }

        [Display(Name = "Tags")]
        public List<ProfileTags> Tags { get; set; }


        public void AddProfToDB(Profile profile)
        {
            var constants = new Constants();
            string stringTags = profile.Tags[0].TagName;
            var listTags = profile.Tags;
            for (int i = 1; i < listTags.Count; i++)
            {
                stringTags += "," + listTags[i].TagName;
            }
            if (HttpContext.Session["LoggedIn"])
            {

                using (var cn = new SqlConnection(Constants.ConnString))
                {
                    string _sql = (@"SELECT * FROM [dbo].[Users] WHERE username = @username");
                    var cmd = new SqlCommand(_sql, cn);
                    cmd.Parameters.AddWithValue("@username", constants.UserName);
                    cn.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        cn.Close();
                        string _sqlInsert = @"INSERT INTO [dbo].[Profile] (userid, gender, sexpref, bio, tags) VALUES (@userid, @gender, @sexpref, @bio, @tags)";
                        cmd = new SqlCommand(_sqlInsert, cn);
                        cmd.Parameters.AddWithValue("@userid", "11");
                        cmd.Parameters.AddWithValue("@gender", profile.Gender);
                        cmd.Parameters.AddWithValue("@sexpref", profile.SexPref);
                        cmd.Parameters.AddWithValue("@bio", profile.Bio);
                        cmd.Parameters.AddWithValue("@tags", stringTags);
                        cn.Open();
                        cmd.ExecuteReader();
                        System.Diagnostics.Debug.WriteLine("ADDED");
                    }
                    cn.Close();


                }

            }

        }
    }

    public class ProfileTags
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
        public bool IsSelected { get; set; }
    }

    public class ProfileTagsList
    {
        public List<ProfileTags> DBTags { get; set; }
    }
}