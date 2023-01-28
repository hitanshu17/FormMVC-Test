using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormMVC.Controllers
{
    public class HomeController : Controller
    {
        //private FormMVC db = new FormMVC();
        public ActionResult Index()
        {
            //return View(db.form.ToList());
            return View();
        }

        public ActionResult Create()
        {
            var countries = new List<SelectListItem>()
            {
                new SelectListItem { Text = "India", Value = "IN" },
                new SelectListItem { Text = "Australia", Value = "AUS" },
                new SelectListItem { Text = "France", Value = "FR" },
            };
            ViewBag.Countries = countries;

            var courses = new List<SelectListItem>()
            {
                new SelectListItem { Text = " B.TECH ", Value = "1" },
                new SelectListItem { Text = " BCA ", Value = "2" },
                new SelectListItem { Text = " B.COM ", Value = "3" },
                new SelectListItem { Text = " BBA ", Value = "4" },
                new SelectListItem { Text = " LLB ", Value = "5" },
            };
            ViewBag.courses = courses;
            return View();

        }

        /*[HttpPost]
        public JsonResult Save(party_master pm)
        {
            try
            {
                var c_ns = System.Configuration.ConfigurationManager.ConnectionStrings["usercheck"].ToString();
                c_ns += "MultipleActiveResultSets=True;";
                using (SqlConnection connection = new SqlConnection(c_ns))
                {
                    String query = "INSERT INTO PARTY_MASTER (company_code,type,address,phone,email,person_designation,remarks,party_name,party_code) VALUES ('C01',@party_type,@party_address,@party_phone,@party_email,@party_desgination, @party_remarks,@party_name,@party_code)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@party_type", pm.type);
                        command.Parameters.AddWithValue("@party_address", pm.address ?? " ");
                        command.Parameters.AddWithValue("@party_person", pm.person ?? " ");
                        command.Parameters.AddWithValue("@party_desgination", pm.person_designation ?? " ");
                        command.Parameters.AddWithValue("@party_remarks", pm.remarks);
                        command.Parameters.AddWithValue("@party_phone", pm.phone ?? 0);
                        command.Parameters.AddWithValue("@party_email", pm.email ?? " ");
                        command.Parameters.AddWithValue("@party_name", pm.party_name);
                        command.Parameters.AddWithValue("@party_code", pm.party_code);
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new JsonResult { };
        }*/
    }
}