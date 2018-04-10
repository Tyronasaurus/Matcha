using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Login2.Models;

namespace Login2.Models
{
    public class User
    {

        //------LOGIN-----\\

        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me on this computer")]
        public bool RememberMe { get; set; }

        public bool IsValid(string _username, string _password)
        {
            using (var cn = new SqlConnection(Constants.ConnString))
            {
                string _sql = @"SELECT [username] FROM [dbo].[Users] WHERE [username] = @user AND [password] = @pass";
                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@user", _username);
                cmd.Parameters.AddWithValue("@pass", Helpers.SHA1.Encode(_password));
                cn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }
                else
                { 
                    return false;
                }
            }
        }

        //-----REGISTER-----\\

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        private string token;
        public string Token
        {
            get
            {
                return (token);
            }
            set
            {
                token = Guid.NewGuid().ToString();
            }
        }

        public bool Registration(User user)
        {
            user.Token = Guid.NewGuid().ToString();
            using (var cn = new SqlConnection(Constants.ConnString))
            {
                string _sql = @"SELECT * FROM [dbo].[Users] WHERE username = @uname OR email = @email";
                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@uname", SqlDbType.VarChar))
                    .Value = user.Username;
                cmd.Parameters
                    .Add(new SqlParameter("@email", SqlDbType.VarChar))
                    .Value = user.Email;
                cn.Open();

                var x = cmd.ExecuteScalar();
                if (x == null)
                {
                    string _sqlinsert = @"INSERT INTO [dbo].[Users] (email, username, first_name, last_name, password, token) VALUES (@email, @user, @fname, @lname, @pass, @token)";
                    cmd = new SqlCommand(_sqlinsert, cn);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@user", user.Username);
                    cmd.Parameters.AddWithValue("@fname", user.FirstName);
                    cmd.Parameters.AddWithValue("@lname", user.LastName);
                    cmd.Parameters.AddWithValue("@pass", Helpers.SHA1.Encode(user.Password));
                    cmd.Parameters.AddWithValue("@token", user.Token);
                    //cn.Open();
                    cmd.ExecuteReader();
                }
                else
                {
                    return false;
                }
                cn.Close();
                return true;
            }
        }
        //------PROFILE STUFF------\\
    }
}