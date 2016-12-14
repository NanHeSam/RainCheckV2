using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RainCheckV2.Models;

namespace RainCheckV2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getQuote()
        {
            // when validation finished
            TempData["zipcode"] = Request.Form["Zipcode"];

            return RedirectToAction("../GetQuote/Get");
        }

        public ActionResult getSavedQuotePage()
        {
            return View("getSavedQuote");
        }

        public ActionResult getSavedQuote()
        {
            //check if saved quote exists
            string referenceNum = Request.Form["Reference_number"];
            RainCheckServerEntities entity = new RainCheckServerEntities();
            List<quote> targets = entity.quotes.Where(x => x.Reference_number.Equals(referenceNum)).ToList();

            if (targets.Count() == 0)
            {
                TempData["Message"] = "Quote not Exist!";
                return RedirectToAction("getSavedQuotePage");
            }
            else
            {
                //TempData["quoteId"] = targets[0].quote_id;
                //TempData["referenceNum"] = targets[0].Reference_number;
                TempData["quote_display"] = targets[0];
                return View("displayQuote", targets[0]);

            }

        }

        public ActionResult gotoPurchase(quote qt)
        {
            qt = (quote)TempData["quote_display"];
            TempData["quoteId"] = qt.quote_id;
            TempData["referenceNum"] = qt.Reference_number;
            return RedirectToAction("../CarInfo/CarInfo");
        }
    }
}