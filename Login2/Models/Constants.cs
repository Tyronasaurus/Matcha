using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Login2.Models
{
    public class Constants
    {
        public const string ConnString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tyron\source\repos\Matcha\Login2\App_Data\Accounts.mdf;Integrated Security=True";

        public string Tags()
        {
            string tagString = null;
            using (var cn = new SqlConnection(ConnString))
            {
                string _sql = @"SELECT TagName FROM [dbo].[Tags]";
                var cmd = new SqlCommand(_sql, cn);
                cn.Open();
                var reader = cmd.ExecuteReader();
                reader.Read();
                tagString = reader.GetValue(0).ToString();
                while (reader.Read())
                {
                    tagString += ',' + reader.GetValue(0).ToString();
                }
            }
            return (tagString);
        }
    }

}