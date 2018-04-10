using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Login2.Controllers
{
    public class VerifyController : Controller
    {
        // GET: Verify
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Verify ()
        {
            string verif = Request.QueryString["token"];
            
            return View(User);
        }
    }
}