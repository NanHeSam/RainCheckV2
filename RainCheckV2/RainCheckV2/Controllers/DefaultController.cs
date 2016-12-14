using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RainCheckV2.Models;

namespace RainCheckV2.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index() 
        {
            var context = new RainCheckServerEntities();
            var stateObject = context.states.ToList<state>();
            return View(stateObject);
        }
    }
}