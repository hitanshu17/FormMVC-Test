using System;

namespace FormMVC.ViewModel
{
    public class FormModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public Nullable<int> mobile { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string enter_by { get; set; }
        public int[] courses { get; set; }
        public string coursesView { get; set; }
    }
}