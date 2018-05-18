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
        SqlDataReader reader = null;
        SqlConnection cn = null;
        SqlCommand cmd = null;
        HttpContext context = HttpContext.Current;

        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Fame")] 
        public int Fame { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Sexual Preference")]
        public string SexPref { get; set; }

        [Display(Name = "Biography")]
        public string Bio { get; set; }

        [Display(Name = "Tags")]
        public List<ProfileTags> Tags{ get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Online")]
        public bool Online { get; set; }

        [Display(Name = "Last Online")]
        public string LastOnline { get; set; }

        public bool AddProfToDB(Profile profile)
        {
          
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
                    Username = context.Session["Username"].ToString();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        cn.Close();
                        System.Diagnostics.Debug.WriteLine("Username in users");
                        string sql = AddUpdateProfDB(profile);
                        cmd = new SqlCommand(sql, cn);
                        cmd.Parameters.AddWithValue("@username", Username);
                        cmd.Parameters.AddWithValue("@fname", profile.FirstName);
                        cmd.Parameters.AddWithValue("@lname", profile.LastName);
                        cmd.Parameters.AddWithValue("@age", (int)profile.Age);
                        cmd.Parameters.AddWithValue("@gender", profile.Gender);
                        cmd.Parameters.AddWithValue("@sexpref", profile.SexPref);
                        cmd.Parameters.AddWithValue("@bio", profile.Bio);
                        cmd.Parameters.AddWithValue("@tags", stringTags);
                        cmd.Parameters.AddWithValue("@location", profile.Location);
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
                    System.Diagnostics.Debug.WriteLine("AddProfToDB: " + e.Message);
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
                    return (@"UPDATE Profile SET first_name = @fname, last_name = @lname, " +
                            "age = @age, gender = @gender, sexPref = @sexPref, bio = @bio, " +
                            "tags = @tags, location = @location WHERE username = @username");
                }
                else
                {
                    cn.Close();
                    System.Diagnostics.Debug.WriteLine("Inserting Profile DB");
                    return (@"INSERT INTO Profile " +
                            "(username, first_name, last_name, age, gender, sexpref, bio, tags, location) " +
                            "VALUES (@username, @lname, @age, @fame, @gender, @sexpref, @bio, @tags, @location)");
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        public int GetProfileCount()
        {
            int ProfCount = 0;
            cn = new SqlConnection(Constants.ConnString);
            string _sql = @"SELECT COUNT(*) AS ProfCount FROM Profile";
            cmd = new SqlCommand(_sql, cn);
            cn.Open();
            reader = cmd.ExecuteReader();
            reader.Read();
            ProfCount = (int)reader["ProfCount"];
            cn.Close();
            return ProfCount;

        }

        public int CalcFame(string Username)
        {
            Like likeModel = new Like();
            int Likes = likeModel.LikeCount(Username);
            int ProfCount = GetProfileCount();
            decimal res = ((decimal)Likes / ProfCount) * 100;
            Fame = (int)res;
            System.Diagnostics.Debug.WriteLine(Likes + " / " + ProfCount + " * 100 = " + res);
            return Fame;
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