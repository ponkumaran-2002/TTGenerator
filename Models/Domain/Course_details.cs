using System.ComponentModel.DataAnnotations;

namespace TTGenerator.Models.Domain
{
    public class Course_details
    {
        [Key]
        public string Course_id { get; set; }
        public string Course_name { get; set; }
        public string Department { get; set; }
        public int L { get; set; }
        public int T { get; set; }
        public int P { get; set; }
        public Double C { get; set; }
        public int Course_type { get; set; }    

    }
}
