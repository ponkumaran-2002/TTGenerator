using System.ComponentModel.DataAnnotations.Schema;

namespace TTGenerator.Models
{
    public class Add_course_details
    {

        public string Course_id { get; set; }
        public string Course_name { get; set; }
        public string Department { get; set; }
        public int L { get; set; }
        public int T { get; set; }
        public int P { get; set; }
        public float C { get; set; }
        public int Course_type { get; set; }
    }
}

