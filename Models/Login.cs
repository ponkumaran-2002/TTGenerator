using System.ComponentModel.DataAnnotations;

namespace TTGenerator.Models
{
    public class Login
    {
        [Key]
        public string login_id { get; set; }
        public string password_l { get; set; }
        public string faculty_id { get; set; }
        public string roleid { get; set; }
    }
}
