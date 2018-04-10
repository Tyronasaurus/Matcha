using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Login2.Models
{
    public class Profile
    {
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Sexual Preference")]
        public string SexPref { get; set; }

        [Display(Name = "Biography")]
        public string Bio { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        public List<string> SexAndGender = new List<string>(new string[] { "Male", "Female", "Other" });

    }
}