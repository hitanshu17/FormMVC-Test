using FormMVC.Models;
using FormMVC.ViewModel;
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
            List<form_master> data = db.form_master.ToList();
            List<FormModel> model = new List<FormModel>();
            foreach (var item in data)
            {
                List<form_detail> detail = db.form_detail.Where(p => p.form_id == item.id).ToList();
                var d = detail.Select(p => p.course.ToString()).ToArray();
                model.Add(new FormModel { id = item.id, name = item.name, email = item.email, address = item.address, mobile = item.mobile, country = item.country, state = item.state, city = item.city, coursesView = string.Join(",", d) });
            }
            return View(model);
            //return View(db.form_master.ToList());
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
        public JsonResult Save(FormModel fm)
        {
            try
            {
                accountEntities db = new accountEntities();
                int result = 0;
                var c_ns = System.Configuration.ConfigurationManager.ConnectionStrings["usercheck"].ToString();
                c_ns += "MultipleActiveResultSets=True;";
                using (SqlConnection connection = new SqlConnection(c_ns))
                {
                    String query = "INSERT INTO FORM_MASTER (name,email,address,mobile,country,state,city,enter_by) VALUES (@name,@email,@address,@mobile,@country,@state,@city,@enter_by); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", fm.name);
                        command.Parameters.AddWithValue("@email", fm.email ?? " ");
                        command.Parameters.AddWithValue("@address", fm.address ?? " "); //string.Empty
                        command.Parameters.AddWithValue("@mobile", fm.mobile ?? 0);
                        command.Parameters.AddWithValue("@country", fm.country ?? " ");
                        command.Parameters.AddWithValue("@state", fm.state ?? " ");
                        command.Parameters.AddWithValue("@city", fm.city ?? " ");
                        command.Parameters.AddWithValue("@enter_by", fm.enter_by ?? " ");
                        connection.Open();
                        //result = command.ExecuteNonQuery();
                        result = Convert.ToInt32(command.ExecuteScalar());
                        if (result > 0)
                        {
                            foreach (var course in fm.courses)
                            {
                                command.Parameters.Clear();
                                query = "INSERT INTO FORM_DETAIL (form_id,course) VALUES (@form_id,@course)";
                                command.CommandText = query;
                                command.Parameters.AddWithValue("@form_id", result);
                                command.Parameters.AddWithValue("@course", course);
                                command.ExecuteNonQuery();
                            }

                            return Json(true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Json(false);
        }

        public ActionResult Help()
        {
            return View();

        }

        /*public JsonResult delete(String pcode)
        {
            form_master delete = db.form_master.Find(pcode);
            db.form_master.Remove(delete);
            db.SaveChanges();
            return Json(delete);
        }*/

    }
}