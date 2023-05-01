using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace TTGenerator.Models
{
    [Keyless]

    public class View_tt
    {


        public int semester_id { get; set; }
        public string year_s { get; set; }
        public int batch_start_year { get; set; }
    }
        
}
