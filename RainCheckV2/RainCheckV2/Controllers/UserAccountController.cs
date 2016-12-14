using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using RainCheckV2.Models;
using RainCheckV2.ViewModels;

namespace RainCheckV2.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount

        private RainCheckServerEntities context;

        public UserAccountController()
        {
            context = new RainCheckServerEntities();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

        public ActionResult UserMain()
        {
            Session["logged_username"] = "Sam";
            var policy = new policyViewModel(1);
            return View(policy);
        }



        public ActionResult Submit()
        {
            List<coverage_level> coverage = context.coverage_level.ToList<coverage_level>();
            return View("ChangePolicy", coverage);
        }

        //[HttpPost]
        //public ActionResult saveQuote(string save, string cancel)
        //{
        //    var policies = new policyViewModel(1);
        //    if (!string.IsNullOrEmpty(save))
        //    {
        //        var policy = context.policy_tbl.Where(o => o.user_id == 1).OrderByDescending(o => o.start_date).First();

        //        policy.self_body = System.Convert.ToDecimal(Request.Form["Self_body"]);
        //        policy.self_property = System.Convert.ToDecimal(Request.Form["Self_property"]);
        //        policy.opposite_body = System.Convert.ToDecimal(Request.Form["op_body"]);
        //        policy.opposite_property = System.Convert.ToDecimal(Request.Form["op_property"]);
        //        context.SaveChanges();
        //        return RedirectToAction("UserMain", policies);
        //    }
        //    else if (!string.IsNullOrEmpty(cancel))
        //    {
        //        return View("UserMain", policies);
        //    }
        //    return RedirectToAction("Submit");

        //}
    }
}