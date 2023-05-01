using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace TTGenerator.Models.Domain
{

    public class allot_lab
    {
        [Key]
        public int sno { get; set; }
        public int semester_id { get; set; }
        public string year_s { get; set; }
        public int batch_start_year { get; set; }
        public int slot { get; set; }
        public string day_tt { get; set; }
        public string course_id { get; set; }
        public string dept { get; set; }
        public string sec { get; set; }
    }
}
