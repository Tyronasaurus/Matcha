using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;


namespace Matcha.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                dynamic viewBag = new DynamicDictionary();
                viewBag.WelcomeMessage = "Welcome to Matcha!";
                viewBag.ErrorMessage = "This is an error";
                return View["home", viewBag];
            };
        }
    }
}