using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TTGenerator.Models.Domain
{
    
    public partial class clg_tt_level
    {
           [Key]
           public int sno { get; set; }
           public int semester_id { get; set; }
           public string year_s { get; set; }
           public int batch_start_year { get; set;}
           public int slot { get; set; }
           public string day_tt { get; set; }
           public int elective_id { get; set; }
    }
}
