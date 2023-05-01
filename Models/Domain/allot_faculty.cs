using System.ComponentModel.DataAnnotations;

namespace TTGenerator.Models.Domain
{
   // [HasNoKey]   
    public class allot_faculty
    {
        [Key]
        public string Faculty_id { get; set; }
        public string Course_id { get; set; }
        public string S_year { get; set; }
        public int Batch { get; set; }
        public string Section { get; set; }
        public int Semester { get; set; }
    }
}
