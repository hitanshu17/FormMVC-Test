using FormMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace FormMVC.Controllers
{
    public class HomeController : Controller
    {
        private accountEntities db = new accountEntities();
        public ActionResult Index()
        {
            return View(db.form_master.ToList());
            //return View();
        }

        public ActionResult Create()
        {
            var countries = new List<SelectListItem>()
            {
                new SelectListItem { Text = "India", Value = "0" },
                new SelectListItem { Text = "Australia", Value = "1" },
                new SelectListItem { Text = "France", Value = "2" },
            };
            ViewBag.Countries = countries;

            var courses = new List<SelectListItem>()
            {
                new SelectListItem { Text = " BTECH ", Value = "0" },
                new SelectListItem { Text = " BCA ", Value = "1" },
                new SelectListItem { Text = " BCOM ", Value = "2" },
                new SelectListItem { Text = " BBA ", Value = "3" },
                new SelectListItem { Text = " LLB ", Value = "4" },
            };
            ViewBag.courses = courses;
            return View();

        }

        //Code to get states according to country name

        [HttpGet]
        public JsonResult GetStates(int country)
        {

            List<SelectListItem> states = new List<SelectListItem>();

            switch (country)
            {
                case 0:
                    states.Add(new SelectListItem { Text = "Delhi", Value = "DEL" });
                    states.Add(new SelectListItem { Text = "Uttar Pradesh", Value = "UP" });
                    states.Add(new SelectListItem { Text = "Punjab", Value = "PUN" });
                    states.Add(new SelectListItem { Text = "Haryana", Value = "HAR" });
                    break;
                case 1:
                    states.Add(new SelectListItem { Text = "Queensland", Value = "QUEEN" });
                    states.Add(new SelectListItem { Text = "New South Walves", Value = "NSW" });
                    states.Add(new SelectListItem { Text = "Victoria", Value = "VIC" });
                    break;
                case 2:
                    states.Add(new SelectListItem { Text = "Auvergne-Rhône-Alpes", Value = "ARA" });
                    states.Add(new SelectListItem { Text = "Bourgogne-Franche-Comté", Value = "BFC" });
                    states.Add(new SelectListItem { Text = "Grand Est", Value = "GRET" });
                    states.Add(new SelectListItem { Text = "Paris", Value = "PAR" });
                    break;
            }
            return Json(states, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult checkEmail(String email)
        {
            accountEntities db = new accountEntities();
            var c_ns = System.Configuration.ConfigurationManager.ConnectionStrings["usercheck"].ToString();
            c_ns += "MultipleActiveResultSets=True;";
            using (SqlConnection connection = new SqlConnection(c_ns))
            {
                var isExist = db.form_master.Any(x => x.email == email);
                return Json(isExist, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult Save(form_master fm, form_detail fd)
        {
            try
            {
                accountEntities db = new accountEntities();
                int result = 0;
                var c_ns = System.Configuration.ConfigurationManager.ConnectionStrings["usercheck"].ToString();
                c_ns += "MultipleActiveResultSets=True;";
                using (SqlConnection connection = new SqlConnection(c_ns))
                {
                    String query = "INSERT INTO FORM_MASTER (name,email,address,mobile,country,state,city,enter_by) VALUES (@name,@email,@address,@mobile,@country,@state,@city,@enter_by)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", fm.name);
                        command.Parameters.AddWithValue("@email", fm.email ?? " ");
                        command.Parameters.AddWithValue("@address", fm.address ?? " ");
                        command.Parameters.AddWithValue("@mobile", fm.mobile ?? 0);
                        command.Parameters.AddWithValue("@country", fm.country ?? " ");
                        command.Parameters.AddWithValue("@state", fm.state ?? " ");
                        command.Parameters.AddWithValue("@city", fm.city ?? " ");
                        command.Parameters.AddWithValue("@enter_by", fm.enter_by ?? " ");
                        connection.Open();
                        result = command.ExecuteNonQuery();
                    }
                    /*if (result > 0)
                    {
                        foreach (var it in fd)
                        {
                            it.balance_qty = it.item_qty;
                            query = "INSERT INTO FORM_DETAIL (course) VALUES (@course)";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                //command.Parameters.AddWithValue("@form_id", fm.name);
                                command.Parameters.AddWithValue("@course", fd.course);
                                connection.Open();
                                result = command.ExecuteNonQuery();
                            }
                        }
                        return Ok(true);
                    }*/

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new JsonResult { };
        }

    }
}