using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RainCheckV2.Models;
using System.Net;
using System.Net.Mail;

namespace RainCheckV2.Controllers
{
    public class LoginController : Controller
    {
        private RainCheckServerEntities entity = new RainCheckServerEntities();
        // GET: Login
        public ActionResult LoginPage()
        {
            login obj = new login();
            HttpCookie cookie = Request.Cookies["login_info"];
            if (cookie != null)
            {
                obj.user_name = cookie["username"];
                obj.password = cookie["password"];
            }
            return View("LoginPage", obj);
        }
    //    [Route("UserAccount/UserMain")]
        public ActionResult LoginSubmit(login lgn, bool RememberCheckbox = false)
        {

            var username = lgn.user_name;

            // if checkbox is checked, save username to cookie
            if (RememberCheckbox)
            {
                HttpCookie cookie = new HttpCookie("login_info");
                cookie["username"] = username;
                cookie["password"] = lgn.password;
                cookie.Expires = DateTime.Now.AddMinutes(2);
                Response.Cookies.Add(cookie);
            }

            List<login> targets = entity.logins.Where(x => x.user_name.Equals(username)).ToList(); // find record with certain username

            if (targets.Count() == 0) // wrong username
            {
                TempData["errorMessage"] = "No such user!";
                return RedirectToAction("LoginPage");
            }
            else if (!lgn.password.Equals(targets[0].password)) // wrong password
            {
                TempData["errorMessage"] = "Wrong password and username combination";
                return RedirectToAction("LoginPage");
            }
            // login success:
            Session["logged_username"] = targets[0].user_name; // store logined username
            Session["custId"] = targets[0].customer_id; // store customer id
            return RedirectToAction("../UserAccount/UserMain"); // this part is for test. Need to redirect to ../UserAcc/display something
        }

        public ActionResult ForgetUsername()
        {

            return View();
        }

        public ActionResult DisplayUsername(forgetUsernameVM vm)
        {
            string email = vm.email; // Request.Form["Email"];
            decimal policyNum = vm.policy_Num; // Convert.ToDecimal( Request.Form["Policy"]);
            var usernames = from c in entity.customer_tbl
                            join u in entity.user_tbl on c.userid equals u.userid
                            join l in entity.logins on c.customer_id equals l.customer_id
                            join p in entity.policy_tbl on u.userid equals p.user_id
                            where u.user_email == email && p.policy_number == policyNum
                            select new { l.user_name };

            if (usernames.Count() == 0)
            {
                TempData["errorMessage"] = "No such user Or \n wrong combination of email and policy number";
                return RedirectToAction("ForgetUsername");
            }

            // if inputed information is incorrect
            // return View("ForgetUsername");

            ViewData["username"] = "Your username is " + usernames.ToList()[0].user_name + ".";
            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> SendEmailResetPassword(fogetPasswordVM vm)
        {
            string username = vm.username;// Request.Form["username"]; // 
            decimal policyNum = vm.policy_Num;// Convert.ToDecimal(Request.Form["policy_Num"]);



            var targets = from l in entity.logins
                          join c in entity.customer_tbl on l.customer_id equals c.customer_id
                          join u in entity.user_tbl on c.userid equals u.userid
                          join p in entity.policy_tbl on u.userid equals p.user_id
                          where l.user_name == username && p.policy_number == policyNum
                          select new { u.user_email, l.user_name };

            if (targets.Count() == 0)
            {// if inputed information is incorrect

                TempData["errorMessage"] = "No such user or \n wrong combination of username and policy number";
                return RedirectToAction("ForgetPassword");
            }
            else
            {// else
             // email verification code to user email address and then
             // display reset password form, with verification code.

                string email = targets.ToList()[0].user_email;
                Random rnd = new Random();
                int vCode = rnd.Next(1001, 10000);

                // send email to user
                MailMessage message = new MailMessage();
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress("rainchecksuper@gmail.com");
                message.Subject = "Verification code from RainCheck";
                message.Body = String.Format("Your verification code is {0}. Code will expire in 10 minutes.", vCode);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "rainchecksuper@gmail.com",
                        Password = "raincheck2000"
                    };

                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);

                }
                Session["verificationCode"] = vCode;
                Session["resetPswd_username"] = username; // store username of user who want to reset password
                TempData["Message"] = "Verification Code sent to your email. \n Please enter code to continue.";
                return RedirectToAction("EnterVerifyCode");
            }

        }

        public ActionResult EnterVerifyCode()
        {
            return View("EnterVerifyCode");
        }

        public ActionResult verifyCode()
        {
            if (Session["verificationCode"] == null)
            {
                TempData["errorMessage"] = "Session ended, re-enter to found password";
                return RedirectToAction("ForgetPassword");
            }
            if (!Request.Form["VerificationCode"].Equals(Session["verificationCode"].ToString()))
            {
                TempData["errorMessage"] = "Verification Code invalid! Please enter again.";
                return RedirectToAction("EnterVerifyCode");
            }

            else
            {
                return RedirectToAction("ResetPassword");
            }

        }

        public ActionResult ResetPassword()
        {
            if (Session["resetPswd_username"] == null) // session set after emailed code
            {// session expired, return to forget password page.
                return RedirectToAction("ForgetPassword");

            }
            var username = Session["resetPswd_username"].ToString();

            // validate password and confirm password
            string password = Request.Form["NewPassword"];
            string confirmPassword = Request.Form["ConfirmPassword"];

            // if both input is null, this call is from verifyCode
            if (password == null && confirmPassword == null)
            {
                return View();
            }

            // server sider verification
            if (password == "" || confirmPassword == "")
            {
                TempData["Message"] = "New password and confirm password cannot be empty!";
                return View();
            }
            if (!password.Equals(confirmPassword))
            {
                TempData["Message"] = "New password and confirm password not match!";
                return View();
            }
            // reset password
            login target = entity.logins.Where(x => x.user_name == username).ToList()[0];
            target.password = password;
            entity.SaveChanges();

            TempData["Message"] = "Your password successfully reset.";
            return View("ResetPasswordResult");
        }
    }
}