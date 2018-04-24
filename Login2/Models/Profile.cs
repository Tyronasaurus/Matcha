using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Login2.Models
{
    public class BigModel
    {
        public Profile Profile { get; set; }
        public ProfileTagsList ProfileTagsList { get; set; }
    }

    public class Profile
    {
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Sexual Preference")]
        public string SexPref { get; set; }

        [Display(Name = "Biography")]
        public string Bio { get; set; }

        [Display(Name = "Tags")]
        public List<ProfileTags> Tags{ get; set; }


        public bool AddProfToDB(Profile profile)
        {
            SqlDataReader reader = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            HttpContext context = HttpContext.Current;
            var constants = new Constants();

            string stringTags = profile.Tags[0].TagName;
            var listTags = profile.Tags;

            for (int i = 1; i < listTags.Count; i++)
            {
                stringTags += "," + listTags[i].TagName;
            }

            var isLogged = context.Session["LoggedIn"];
            if (isLogged != null)
            {
                try
                {
                    cn = new SqlConnection(Constants.ConnString);
                    string _sql = (@"SELECT username FROM Users WHERE username = @username");
                    cmd = new SqlCommand(_sql, cn);
                    cmd.Parameters.AddWithValue("@username", context.Session["Username"]);
                    System.Diagnostics.Debug.WriteLine(context.Session["Username"]);
                    cn.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        cn.Close();
                        System.Diagnostics.Debug.WriteLine("Username in users");
                        string sql = AddUpdateProfDB(profile);
                        cmd = new SqlCommand(sql, cn);
                        cmd.Parameters.AddWithValue("@username", context.Session["Username"]);
                        cmd.Parameters.AddWithValue("@gender", profile.Gender);
                        cmd.Parameters.AddWithValue("@sexpref", profile.SexPref);
                        cmd.Parameters.AddWithValue("@bio", profile.Bio);
                        cmd.Parameters.AddWithValue("@tags", stringTags);
                        cn.Open();
                        cmd.ExecuteReader();
                        cn.Close();
                        System.Diagnostics.Debug.WriteLine("Added to DataBase");
                        return true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Didnt find username in users");
                        cn.Close();
                        return false;
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    cn.Close();
                    return false;
                }
            }
            else
                System.Diagnostics.Debug.WriteLine("Not logged in");
                return false;
        }

        public string AddUpdateProfDB(Profile profile)
        {
            HttpContext context = HttpContext.Current;
            try
            {
                var cn = new SqlConnection(Constants.ConnString);
                string sql = @"SELECT * FROM Profile WHERE username = @username";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@username", context.Session["Username"]);
                cn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    cn.Close();
                    System.Diagnostics.Debug.WriteLine("Updating Profile DB");
                    return (@"UPDATE Profile SET gender = @gender, sexPref = @sexPref, bio = @bio, tags = @tags " +
                        @"WHERE username = @username");
                }
                else
                {
                    cn.Close();
                    System.Diagnostics.Debug.WriteLine("Inserting Profile DB");
                    return (@"INSERT INTO Profile (username, gender, sexpref, bio, tags) VALUES (@username, @gender, @sexpref, @bio, @tags)");
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
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