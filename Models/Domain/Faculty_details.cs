//using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTGenerator.Models.Domain
{
    public partial class Faculty_details
    {
       
       [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key] public string faculty_id { get; set; }
      // public string faculty_id { get; set; }
       public string faculty_name { get;set; }
       public int faculty_phno { get; set; }   
       public string faculty_email {get; set; }
       public DateTime faculty_dob { get; set; }
       public string faculty_address_dno { get; set; }
       public string faculty_address_place { get; set; }
       public string faculty_address_pincode { get; set; }
       public string faculty_dept { get; set; }


        
    }
}
