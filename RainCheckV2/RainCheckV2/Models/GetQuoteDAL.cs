using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RainCheckV2.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace RainCheckV2.Models
{
    public class GetQuoteDAL
    {
        RainCheckServerEntities rcsobject = new RainCheckServerEntities();


        public Array Getmodels(string make_name)
        {
            ObjectResult<string> model_name = rcsobject.RCSgetmodel(make_name);
            string[] models = model_name.ToArray<string>();
            return models;
        }

        public Array GetStates()
        {
            ObjectResult<string> getstates = rcsobject.SP_GetState();
            string[] states = getstates.ToArray<string>();
            return states;
        }

        public Array GetCities(string statename)
        {
            ObjectResult<string> getcities = rcsobject.SP_GetCity(statename);
            string[] cities = getcities.ToArray<string>();
            return cities;
        }

        public int InsertUser(string firstname, string middlename, string lastname, decimal phone, string ssn, System.DateTime dob, string email, string address, string city, string state, decimal zip, bool marital_Status, bool gender, string education_Level, string employment, bool accident_Records_5YEARS, bool traffic_Tickets_5YEARS, bool dUIs_10_YEARS, bool suspensions_5YEARS, bool defensive_Drive_2YEARS, bool military_Affiliation, bool veteran_Status, decimal rank_ID)
        {
            ObjectParameter useridDB1 = new ObjectParameter("User_ID", typeof(Int32));
            int UserInfoInserted = rcsobject.SP_InsertUserInfo(firstname, middlename, lastname, phone, ssn, dob, email, address, city, state, zip, marital_Status, gender, education_Level, employment, accident_Records_5YEARS, traffic_Tickets_5YEARS, dUIs_10_YEARS, suspensions_5YEARS, defensive_Drive_2YEARS, military_Affiliation, veteran_Status, rank_ID, useridDB1);
            int userid = (int)useridDB1.Value;
            if (useridDB1 != null)
            {
                return userid;
            }
            else
            {
                return -1;
            }
        }

        public int InsertQuote(int userid, decimal quote_amount, string reference_number)
        {
            decimal useridDB = Convert.ToDecimal(userid);
            ObjectParameter quoteidDB = new ObjectParameter("quoteID", typeof(Int32));
            int UserInfoInserted = rcsobject.SP_InsertQuote(useridDB, quote_amount, reference_number, quoteidDB);
            if (quoteidDB != null)
            {
                int quoteid = (int)quoteidDB.Value;
                return quoteid;
            }
            else
            {
                return -1;
            }
        }

        public int InsertCar(int vin, string car_make, string car_model, string body_style, string primary_use, int estimated_annual_mileage, bool ownership, int year)
        {
            decimal vin_number = Convert.ToDecimal(vin);
            decimal car_year = Convert.ToDecimal(year);
            decimal estimated_annum_mileage = Convert.ToDecimal(estimated_annual_mileage);
            ObjectParameter caridDB = new ObjectParameter("carid", typeof(Int32));
            int CarInfoInserted = rcsobject.SP_InsertCarInfo(vin_number, car_model, body_style, primary_use, estimated_annum_mileage, ownership, car_year, car_make, caridDB);
            if (caridDB != null)
            {
                int carid = (int)caridDB.Value;
                return carid;
            }
            else
            {
                return -1;
            }
        }

        public bool CheckUniqueEmail(string email)  // if it is true tht means the email is unique and proceed!
        {
            int unique = rcsobject.SP_Unique_Email(email).FirstOrDefault() ?? -1;
            if (unique == 0 || unique == -1)
            {
                return true;
            }
            else
                return false;
        }
        public bool CheckUniquePhone(long phone)  // if it is true tht means the email is unique and proceed!
        {
            decimal phonenum = Convert.ToDecimal(phone);
            int unique = rcsobject.SP_Unique_phonenum(phonenum).FirstOrDefault() ?? -1;
            if (unique == 0 || unique == -1)
            {
                return true;
            }
            else
                return false;
        }
        public bool CheckUniqueSSN(string ssn)
        {
            int unique = rcsobject.SP_Unique_SSN(ssn).FirstOrDefault() ?? -1;
            if (unique == 0 || unique == -1)
            {
                return true;
            }
            else
                return false;
        }
    }
}