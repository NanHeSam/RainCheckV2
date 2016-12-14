using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RainCheckV2.Models
{
    public class GetQuoteBLL
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public long PhoneNum { get; set; }
        public string SSN { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Zip { get; set; }
        public bool Marital { get; set; } // true means married & false means single
        public string Gender { get; set; }// 
        public bool GenderDB { get; set; }// true means male & false means female
        public string Education { get; set; }
        public string Employment { get; set; }
        private int Credential { get; set; }
        public bool Accidents { get; set; } //true means yes accidents in ...years
        public bool Tickets { get; set; } //true means yes Tickets in ...years
        public bool DUIs { get; set; } //true means yes DUIs in ...years
        public bool Suspension { get; set; } //true means yes suspension in ...years
        public bool DefensiveDriver { get; set; } //true means yes course taken in ...years
        public bool Military { get; set; }// true means is affiliated.
        public bool Veteran { get; set; } //true means is veteran
        /*********************vehicle**********************************/
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string CarRelation { get; set; } //leased or owned
        public string Usage { get; set; }
        public int Milage { get; set; }

        public decimal basicQuote { get; set; }
        public decimal premiumQuote { get; set; }
        public decimal superQuote { get; set; }


        /*   RainCheckServerEntities1 RainObject = new RainCheckServerEntities1();
           public void insertUser()
           {
               RainObject.user_tbl.Add(); //
           }
           */

        public decimal CalculateQuote()
        {
            Random quote = new Random();
            decimal quote_num = 10000;
            //1)what is the vehicle used for?
            string usage = this.Usage;
            if (usage == "Commute")
            {
                quote_num = quote_num + quote.Next(900, 1000);
            }
            else
            {
                quote_num = (quote_num * 2) + quote.Next(900, 1000);
            }
            //2) enter estimated annual mileage  1 low or 2 medium or 3 high");
            int an_mileage = this.Milage;
            if (an_mileage == 1)
            {
                quote_num = quote_num + quote.Next(900, 1111);
            }
            else if (an_mileage == 2)
            {
                quote_num = quote_num + quote.Next(1111, 2222);
            }
            else
            {
                quote_num = quote_num + quote.Next(2222, 3333);
            }
            //3) enter 1 for married");
            bool relationship = this.Marital;
            if (relationship == true)
            {
                quote_num = quote_num + quote.Next(1000, 2222);
            }
            else
            {
                quote_num = quote_num - quote.Next(999);
            }
            //4) enter 1 for male or 2 for female");
            string gender = this.Gender;
            if (gender == "male")
            {
                GenderDB = true;
                quote_num = quote_num + quote.Next(900, 1000);
            }
            else
            {
                GenderDB = false;
                quote_num = (quote_num) + quote.Next(899);
            }
            //5) enter 1 for veteran or 2 for not veteran");
            bool military = this.Military;
            if (military == true)
            {
                quote_num = quote_num - quote.Next(1111, 2222);
            }
            //6) enter 1 for accident in past 5 years");
            bool accidents = this.Accidents;
            if (accidents == true)
            {
                quote_num = quote_num + quote.Next(1000, 1555);
            }
            //7) enter 1 for traffic tickets in past 5 years");
            bool tickets = this.Tickets;
            if (tickets == true)
            {
                quote_num = quote_num + quote.Next(1111, 2222);
            }
            else
            {
                quote_num = quote_num - quote.Next(999, 1111);
            }
            //8) enter 1 for DUIs in past 10 years");
            bool drunk = this.DUIs;
            if (drunk == true)
            {
                quote_num = quote_num + quote.Next(8888, 9999);
            }
            else
            {
                quote_num = (quote_num) - quote.Next(999);
            }
            //9) enter 1 for Suspension in past 10 years");
            bool suspension = this.Suspension;
            if (suspension == true)
            {
                quote_num = quote_num + quote.Next(9999, 11111);
            }
            //10) enter 1 if taken Safe driver course in past 2 years
            bool safe = this.Suspension;
            if (safe == true)
            {
                quote_num = quote_num - quote.Next(2222, 3333);
            }

            return (quote_num / 100);
        }
    }
}