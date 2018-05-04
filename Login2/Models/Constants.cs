using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Login2.Models
{
    public class Constants
    {
        public const string ConnString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tyron\source\repos\Matcha\Login2\App_Data\Accounts.mdf;Integrated Security=True";
        public const string ipStackToken = "33bb1a67ec223cc1824e90320eb5e962";
    }

}