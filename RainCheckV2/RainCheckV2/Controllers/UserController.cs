using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RainCheckV2.Models;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using RainCheckV2.common;

namespace RainCheckV2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Load()
        {
            customer_tbl ct = new customer_tbl { customer_id = 1, userid = 2, driver_license_number = 22221982, join_date = Convert.ToDateTime("12/12/1982") };
            return View("LoadData", ct);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "StartPolicy")]
        public ActionResult StartPolicy(customer_tbl ob)
        {
            TempData["DriverLicence"] = Request.Form["driver_license_number"];
            RainCheckServerEntities obContext = new RainCheckServerEntities();
            bool uniqDL = true;
            string str = Request.Form["driver_license_number"].ToString();
            //List<customer_tbl> ctb = obContext.customer_tbl.ToList();
            //string st = TempData.Peek("DriverLicence").ToString();
            //decimal temp;
            //foreach (customer_tbl c in ctb)
            //{
            //    if (Decimal.TryParse(st, out temp))
            //    {
            //        if (c.driver_license_number == Decimal.Parse(st))
            //        {
            //            uniqDL = false;
            //            ViewBag.UND = "This Driver has already a policy, Try a diffrent ID";
            //        }
            //    }
            //    else
            //    {
            //        return View("LoadData", ob);
            //    }
            //}

            decimal val;
            if (Decimal.TryParse(str, out val) && str != null)
            {
                List<customer_tbl> ctb = obContext.customer_tbl.ToList();
                foreach (customer_tbl c in ctb)
                {
                    string st = TempData.Peek("DriverLicence").ToString();

                    if (c.driver_license_number == Decimal.Parse(st))
                    {
                        uniqDL = false;
                        ViewBag.UND = "This Driver has already a policy, Try a diffrent ID";
                    }
                }

            }
            else
            {
                uniqDL = false;
            }
            if (ModelState.IsValid && uniqDL)
            {
                policy_tbl pl = new policy_tbl { car_id = 1, start_date = Convert.ToDateTime("12/12/1982"), end_date = Convert.ToDateTime("12/12/1982"), opposite_body = 1, policy_amount = 200, policy_id = 2, self_body = 1, policy_number = 2443662, opposite_property = 2, self_property = 1, user_id = 1 };
                return View("NewPolicy", pl);
            }
            else
            {
                return View("LoadData", ob);
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "PreviousAccount")]
        public ActionResult PreviousAccount()
        {
            policy_tbl pl = new policy_tbl { car_id = 1, start_date = Convert.ToDateTime("12/12/1982"), end_date = Convert.ToDateTime("12/12/1982"), opposite_body = 1, policy_amount = 200, policy_id = 2, self_body = 1, policy_number = 2443662, opposite_property = 2, self_property = 1, user_id = 1 };
            return View("NewPolicy", pl);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CreateAccount")]
        public ActionResult CreateAccount(policy_tbl p)
        {
            

            if (ModelState.IsValid)
            {
                TempData["SD"] = Request.Form["start_date"];
                TempData["ED"] = Request.Form["end_date"];
                TempData["SP"] = Request.Form["SelfProperty"];
                TempData["SB"] = Request.Form["SelfBody"];
                TempData["OP"] = Request.Form["OppositeProperty"];
                TempData["OB"] = Request.Form["OppositeBody"];

                login l = new login { login_id = 1, customer_id = 1, password = " ", user_name = " " };
                return View("NewAccount", l);
            }
            else
            {
                return View("NewPolicy", p);
            }
        }

        public ActionResult Account()
        {
            return RedirectToAction("../UserAccount/UserMain");
        }
        //form submission
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Submit")]
        public ActionResult Submit(login ls)
        {
            RainCheckServerEntities obContext = new RainCheckServerEntities();
            string usn = Request.Form["user_name"].ToString();
            string psw = Request.Form["password"].ToString();
            string cpw = Request.Form["confirms"].ToString();
            bool confirm = false;
            bool samUser = false;
            bool samPassword = false;

            List<login> lgs = obContext.logins.ToList();
            ViewBag.UN = "";
            ViewBag.PW = "";
            ViewBag.CPW = "";
            if (psw == cpw)
            {
                confirm = true;
            }
            else
            {
                ViewBag.CPW = "Your password does not match, please confim it";
            }
            foreach (login l in lgs)
            {
                if (String.Equals(l.user_name, usn))
                {
                    samUser = true;
                    ViewBag.UN = "This Username has been taken, please choose another one";
                }
            }
            foreach (login l in lgs)
            {
                if (String.Equals(l.password, psw))
                {
                    samPassword = true;
                    ViewBag.PW = "This Password has been taken, please choose another one";
                }
            }



            if (ModelState.IsValid && !samPassword && !samUser && confirm)
            {
                //Retrieve the quote object Through TempDate["refNum"]
                RainCheckServerEntities objContext = new RainCheckServerEntities();
                decimal userID = 0;
                // string quoteReference = TempData.Peek("QR").ToString();
                if (TempData.Peek("refNum") == null)
                {
                    TempData["refNum"] = "asdf";
                }
                string quoteReference = TempData.Peek("refNum").ToString();  
                List<quote> qs = objContext.quotes.ToList();
                quote newQuote = new quote();
                foreach (quote q in qs)
                {
                    if (q.Reference_number == quoteReference)
                    {
                        userID = q.userid;
                        newQuote = q;
                    }
                }
                //set up the amount on each coverage for the customer
                decimal policy_amount = 0;
                decimal p1 = getAmount(TempData.Peek("SP").ToString());
                decimal p2 = getAmount(TempData.Peek("SB").ToString());
                decimal p3 = getAmount(TempData.Peek("OP").ToString());
                decimal p4 = getAmount(TempData.Peek("OB").ToString());

                //set up the coverage level for the customer
                decimal selfPro = getCoverage(TempData.Peek("SP").ToString());
                decimal selfBod = getCoverage(TempData.Peek("SB").ToString());
                decimal oppPro = getCoverage(TempData.Peek("OP").ToString());
                decimal oppBod = getCoverage(TempData.Peek("OB").ToString());

                Random rnd = new Random();
                decimal polID = rnd.Next(100000000, 1000000000);
                ViewBag.randm = polID;

                string startDate = TempData.Peek("SD").ToString();
                string endDate = TempData.Peek("ED").ToString();
                ViewBag.st = startDate;
                ViewBag.ed = endDate;
                decimal total = policy_amount + newQuote.quote_amount + p1 + p2 + p3 + p4;
                ViewBag.Amount = policy_amount + newQuote.quote_amount + p1 + p2 + p3 + p4;

                using (RainCheckServerEntities context = new RainCheckServerEntities())
                {
                    try
                    {

                        //Create a new customer in the database  *******************************************/  
                        
                        string dl = TempData.Peek("DriverLicence").ToString();
                        customer_tbl ct = new customer_tbl();
                        ct.userid = userID;
                        ct.join_date = Convert.ToDateTime(startDate);
                        objContext.customer_tbl.Add(ct);
                        ct.driver_license_number = Decimal.Parse(dl);
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (OptimisticConcurrencyException ex)
                        {
                            ex.ToString();
                        }
                    }
                    catch (UpdateException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                //Create a user Login ****************************************************************************/
                decimal custid = 0;
                string un = Request.Form["user_name"].ToString();
                string pw = Request.Form["password"].ToString();
                TempData["PSW"] = pw;
                TempData["USN"] = un;
                TempData["CPW"] = pw;
                List<customer_tbl> customerTable = objContext.customer_tbl.ToList();
                foreach (customer_tbl cs in customerTable)
                {
                    if (cs.customer_id > custid)
                    {
                        custid = cs.customer_id;
                    }
                }

                login lg = new login();
                lg.customer_id = custid;
                lg.user_name = un;
                lg.password = pw;
                obContext.logins.Add(lg);
                // objContext.SaveChanges();
                try
                {
                    objContext.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry

                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Display or log error messages

                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            Console.WriteLine(message);
                        }
                    }
                }

                //Create a new Policy with a new coverage *********************************************************************************/

                //Retrieve the car associted with this policy[getting the car object through session]                
                List<car> crs = objContext.cars.ToList();
                car cr = new car();
                // decimal carId = Decimal.Parse(TempData["CarINFO"].ToString());
                decimal vin = 5;
                decimal carid = 0;
                foreach (car x in crs)
                {
                    if (x.vin_number == vin)
                        carid = x.car_id;
                }
                // objContext.cars.Add(cr);
                //objContext.SaveChanges();  
                //set up the policy for a car
                Random rd = new Random();
                policy_tbl policy = new policy_tbl();
                policy.policy_number = polID;
                policy.car_id = Decimal.Parse(TempData.Peek("Carid").ToString());
                policy.user_id = userID; policy.start_date = Convert.ToDateTime(startDate);
                policy.end_date = Convert.ToDateTime(endDate); policy.policy_amount = total; policy.self_property = selfPro;
                policy.self_body = selfBod; policy.opposite_property = oppPro; policy.opposite_body = oppBod;
                objContext.policy_tbl.Add(policy);
                objContext.SaveChanges();

                return View("WelcomePage");
            }
            else
            {
                return View("NewAccount", ls);
            }
        }
        //Get the coverage Level
        public decimal getCoverage(string coverage)
        {
            int cov = 1;
            if (coverage == "0")
            {
                cov = 1;
            }
            else if (coverage == "10000")
            {
                cov = 2;
            }
            else if (coverage == "25000")
            {
                cov = 3;
            }
            else if (coverage == "50000")
            {
                cov = 4;
            }
            else if (coverage == "100000")
            {
                cov = 5;
            }
            return cov;
        }

        //Get the Sucharge Amount for a given policy
        public decimal getAmount(string amt)
        {
            decimal amnt = 0;
            if (amt == "0")
            {
                amnt = 0;
            }
            else if (amt == "10000")
            {
                amnt = 10;
            }
            else if (amt == "25000")
            {
                amnt = 25;
            }
            else if (amt == "50000")
            {
                amnt = 50;
            }
            else if (amt == "100000")
            {
                amnt = 100;
            }

            return amnt;
        }
    }
}