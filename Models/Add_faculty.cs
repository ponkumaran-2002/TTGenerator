using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTGenerator.Models
{
    public class Add_faculty
    {
        [NotMapped]
        public List<Faculty> FacultyCollection { get; set; }
        [NotMapped]
        public List<Course> CourseCollection { get; set; }  
        public string Faculty_id{get;set;}
        //public Add_faculty()
        //{
          //  Faculty_id = new List<SelectListItem>();
        //}

        //[DisplayName("Subjects")]
        //public List<SelectListItem> Faculty_id
        //{
          //  get;
           // set;
        //}

        public string Course_id { get; set;}
        //public IEnumerable<SelectListItem> Faculty_ids { get; set; }
        public string Year { get; set; }
        public int Batch { get; set; }
        public string section { get; set; }
        public int Semester { get; set; }
    }
}
