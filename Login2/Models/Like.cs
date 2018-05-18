using Login2.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Login2.Models
{
    public class Like
    {
        HttpContext context = HttpContext.Current;
        SqlDataReader reader = null;
        SqlConnection cn = null;
        SqlCommand cmd = null;

        public bool LikeUnlike(string liked_Profile, string liked_User)
        {
            try
            {
                cn = new SqlConnection(Constants.ConnString);
                string _sql = @"SELECT * FROM Likes WHERE liked_user = @liked_user AND profile_liked = @profile";
                cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@liked_user", liked_User);
                cmd.Parameters.AddWithValue("@profile", liked_Profile);
                cn.Open();
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    //LIKE
                    return (true);
                }
                else
                    //UNLIKE
                    return (false);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return (true);
            }
        }

        public int LikeCount(string Username)
        {
            System.Diagnostics.Debug.WriteLine("Inside LikeCount");

            int LikeCounter = 0;

            try
            {
                cn = new SqlConnection(Constants.ConnString);
                string _sql = @"SELECT COUNT(*) AS LikeCount FROM Likes WHERE profile_liked = @profile";
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
            System.Diagnostics.Debug.WriteLine("LikeCount: " + LikeCounter);
            return (LikeCounter);
        }
    }
}