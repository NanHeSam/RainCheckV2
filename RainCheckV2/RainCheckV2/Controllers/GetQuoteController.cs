using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RainCheckV2.Models;

namespace RainCheckV2.Controllers
{
    public class GetQuoteController : Controller
    {
        // GET: GetQuote
        // GET: GetQuote
        public ActionResult Index()
        {
            return RedirectToAction("Get", "GetQuote");
        }

        public ActionResult Get()
        {

            GetQuoteDAL modelObj = new GetQuoteDAL();
            modelObj.GetStates();
            return View("GetQuote", modelObj);
        }

        [HttpPost]   //returns car models of particular make.
        public JsonResult MyAjaxCall(string value)
        {
            GetQuoteDAL modelObject = new GetQuoteDAL();
            // modelObject.Getmake(value);
            return Json(modelObject.Getmodels(value));
        }
        [HttpPost] //returns cities under given state// sorry abt the name of the function.
        public JsonResult MyGetState(string value)
        {
            GetQuoteDAL dalObject = new GetQuoteDAL();
            return Json(dalObject.GetCities(value));
        }
        [HttpPost] //Checks if the email is unique(not existing in database)
        public JsonResult uniqueEmail(string value)
        {
            GetQuoteDAL dalObject = new GetQuoteDAL();
            return Json(dalObject.CheckUniqueEmail(value));
        }

        [HttpPost] //checks if the phone number is unique in the database.
        public JsonResult uniquePhone(string value)
        {
            bool unique = true;
            try
            {
                long phone = Int64.Parse(value);
                GetQuoteDAL dalObject = new GetQuoteDAL();
                unique = dalObject.CheckUniquePhone(phone);
            }
            catch (FormatException)
            {
                Console.WriteLine("The value doesn't match the long format!!!");
            }

            return Json(unique);
        }

        [HttpPost] //checks if the social security number is unique in the database.
        public JsonResult uniqueSSN(string value)
        {
            GetQuoteDAL dalObject = new GetQuoteDAL();
            return Json(dalObject.CheckUniqueSSN(value));
        }

        public ActionResult Reference() //return reference view.
        {
            return View("ReferenceView");
        }
        /********************Go home****************/
        public ActionResult GoHome(string gohome)
        {
            if (!string.IsNullOrEmpty(gohome))
            {
                return RedirectToAction("~/Home/Index");
            }
            else
            {
                return View("ReferenceView");
            }

        }
        /*
         * Inserts the information from the getquote into usertable.
         * Calculate a quote amount using getquoteBLL business logic.
         * Redirects to quoteview view(with different quote name and amount. 
         * */
        public ActionResult Submit()
        {

            GetQuoteBLL qobj = new GetQuoteBLL();
            qobj.FirstName = Request.Form["firstname"];
            qobj.MiddleName = Request.Form["middlename"];
            qobj.LastName = Request.Form["lastname"];
            qobj.Address = Request.Form["streetaddress"];
            qobj.City = Request.Form["city"];
            qobj.State = Request.Form["state"];
            qobj.Zip = Int32.Parse(Request.Form["zip"]);
            qobj.DOB = Convert.ToDateTime(Request.Form["dob"]);
            qobj.SSN = Request.Form["ssn"];
            qobj.Year = Int32.Parse(Request.Form["Year"]);
            qobj.Make = Request.Form["Make"];
            qobj.Model = Request.Form["Model"];
            qobj.CarRelation = Request.Form["vehiclerel"];
            qobj.Usage = Request.Form["usage"];
            qobj.Milage = Int32.Parse(Request.Form["mileage"]);
            try
            {
                qobj.Marital = Convert.ToBoolean(Request.Form["marital"]);
                qobj.Military = Convert.ToBoolean(Request.Form["military"]);
                qobj.Veteran = Convert.ToBoolean(Request.Form["veteran"]);
                qobj.Accidents = Convert.ToBoolean(Request.Form["accidentradio"]);
                qobj.Tickets = Convert.ToBoolean(Request.Form["ticketradio"]);
                qobj.DUIs = Convert.ToBoolean(Request.Form["duiradio"]);
                qobj.Suspension = Convert.ToBoolean(Request.Form["suspensionradio"]);
                qobj.DefensiveDriver = Convert.ToBoolean(Request.Form["defensiveradio"]);
            }
            catch (FormatException)
            {
                Console.WriteLine("The value doesn't match the boolean format!!!");
            }
            qobj.Gender = Request.Form["optradio"];
            qobj.Education = Request.Form["education"];
            qobj.Employment = Request.Form["employment"];

            qobj.Email = Request.Form["email"];
            try
            {
                qobj.PhoneNum = long.Parse(Request.Form["phonenumber"]);
            }
            catch (FormatException)
            {
                Console.WriteLine("The value doesn't match the long format!!!");
            }

            qobj.basicQuote = qobj.CalculateQuote();
            qobj.premiumQuote = qobj.basicQuote + 50;
            qobj.superQuote = qobj.basicQuote + 90;
            TempData["basic"] = qobj.basicQuote;
            TempData["premium"] = qobj.premiumQuote;
            TempData["super"] = qobj.superQuote;

            decimal rank = 0;
            GetQuoteDAL dalObjt = new GetQuoteDAL();

            int userID = dalObjt.InsertUser(qobj.FirstName, qobj.MiddleName, qobj.LastName, qobj.PhoneNum, qobj.SSN, qobj.DOB, qobj.Email, qobj.Address, qobj.City, qobj.State, qobj.Zip, qobj.Marital, qobj.GenderDB, qobj.Education, qobj.Employment, qobj.Accidents, qobj.Tickets, qobj.DUIs, qobj.Suspension, qobj.DefensiveDriver, qobj.Military, qobj.Veteran, rank);
            TempData["Userid"] = userID;

            if (userID == -1)
            {
                GetQuoteDAL modelObj = new GetQuoteDAL();
                modelObj.GetStates();
                return View("GetQuote", modelObj);
            }
            else
            {
                return View("QuoteView", qobj);
            }

        }
        //basic quote amount inserted into quote table, along with other info and if save button checked returns to reference view/ otherwise redirects to carinfo view.
        public ActionResult QuoteInfoA(string basicSave)
        {
            GetQuoteBLL qobj = new GetQuoteBLL();
            GetQuoteDAL dalObject = new GetQuoteDAL();
            decimal quoteAmount = Convert.ToDecimal(TempData.Peek("basic"));
            string tempuserid = Convert.ToString(TempData.Peek("Userid"));
            int userid;
            bool parsed = Int32.TryParse(tempuserid, out userid);
            if (!parsed)
                Console.WriteLine("Int32.TryParse could not parse '{0}' to an int.\n", userid);
            Random random = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 4; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }

            string referencenum = builder.ToString();
            int inserted = dalObject.InsertQuote(userid, quoteAmount, referencenum);
            TempData["refNum"] = referencenum;

            if (inserted == -1)
            {
                GetQuoteDAL modelObj = new GetQuoteDAL();
                modelObj.GetStates();
                return View("GetQuote", modelObj);
            }
            else
            {

                if (!string.IsNullOrEmpty(basicSave))
                {
                    return View("ReferenceView");
                }
                else
                {
                    return RedirectToAction("CarInfo", "CarInfo");

                }
            }
        }

        //premium quote amount inserted into quote table, along with other info and if save button checked returns to reference view/ otherwise redirects to carinfo view.
        public ActionResult QuoteInfoB(string premiumSave)
        {

            GetQuoteDAL dalObject = new GetQuoteDAL();
            decimal quoteAmount = Convert.ToDecimal(TempData.Peek("Premium"));
            string tempuserid = Convert.ToString(TempData.Peek("Userid"));
            int userid;
            bool parsed = Int32.TryParse(tempuserid, out userid);
            if (!parsed)
                Console.WriteLine("Int32.TryParse could not parse '{0}' to an int.\n", userid);

            Random random = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 4; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            string referencenum = builder.ToString();
            int inserted = dalObject.InsertQuote(userid, quoteAmount, referencenum);
            TempData["refNum"] = referencenum;
            if (inserted == -1)
            {
                GetQuoteDAL modelObj = new GetQuoteDAL();
                modelObj.GetStates();
                return View("GetQuote", modelObj);
            }
            else
            {

                if (!string.IsNullOrEmpty(premiumSave))
                {
                    return View("ReferenceView");
                }
                else
                    return RedirectToAction("CarInfo", "CarInfo");
            }
        }
        //super quote amount inserted into quote table, along with other info and if save button checked returns to reference view/ otherwise redirects to carinfo view.
        public ActionResult QuoteInfoC(string superSave)
        {

            GetQuoteDAL dalObject = new GetQuoteDAL();
            decimal quoteAmount = Convert.ToDecimal(TempData.Peek("super"));
            string tempuserid = Convert.ToString(TempData.Peek("Userid"));
            int userid;
            bool parsed = Int32.TryParse(tempuserid, out userid);
            if (!parsed)
                Console.WriteLine("Int32.TryParse could not parse '{0}' to an int.\n", userid);

            Random random = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 4; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }

            string referencenum = builder.ToString();
            int inserted = dalObject.InsertQuote(userid, quoteAmount, referencenum);
            TempData["refNum"] = referencenum;
            if (inserted == -1)
            {
                GetQuoteDAL modelObj = new GetQuoteDAL();
                modelObj.GetStates();
                return View("GetQuote", modelObj);
            }
            else
            {

                if (!string.IsNullOrEmpty(superSave))
                {
                    return View("ReferenceView");
                }
                else
                    return RedirectToAction("CarInfo", "CarInfo");

            }
        }

        /********************testing******************************/

        public ActionResult testingQuoteview()
        {
            GetQuoteBLL testObject = new GetQuoteBLL();
            return View("QuoteView", testObject);
        }
    }
}