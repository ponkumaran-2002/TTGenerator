using System.ComponentModel.DataAnnotations.Schema;
using TTGenerator.Models.Domain;

namespace TTGenerator.Models
{
    public class Allot_lab
    {
        [NotMapped]
        public List<Lab> CourseCollection { get; set; }
        [NotMapped]
        public List<clg_tt_level> Opted_electives { get; set; }
        public int Course_id { get; set; }
        public int semester_id { get; set; }
        public string year_s { get; set; }
        public int batch_start_year { get; set; }
        public string m1 { get; set; }
        public string m2 { get; set; }
        public string m3 { get; set; }
        public string m4 { get; set; }
        public string m5 { get; set; }
        public string m6 { get; set; }
        public string m7 { get; set; }
        public string m8 { get; set; }
        public string m9 { get; set; }
        public string m10 { get; set; }
        public string t1 { get; set; }
        public string t2 { get; set; }
        public string t3 { get; set; }
        public string t4 { get; set; }
        public string t5 { get; set; }
        public string t6 { get; set; }
        public string t7 { get; set; }
        public string t8 { get; set; }
        public string t9 { get; set; }
        public string t10 { get; set; }
        public string w1 { get; set; }
        public string w2 { get; set; }
        public string w3 { get; set; }
        public string w4 { get; set; }
        public string w5 { get; set; }
        public string w6 { get; set; }
        public string w7 { get; set; }
        public string w8 { get; set; }
        public string w9 { get; set; }
        public string w10 { get; set; }
        public string th1 { get; set; }
        public string th2 { get; set; }
        public string th3 { get; set; }
        public string th4 { get; set; }
        public string th5 { get; set; }
        public string th6 { get; set; }
        public string th7 { get; set; }
        public string th8 { get; set; }
        public string th9 { get; set; }
        public string th10 { get; set; }
        public string f1 { get; set; }
        public string f2 { get; set; }
        public string f3 { get; set; }
        public string f4 { get; set; }
        public string f5 { get; set; }
        public string f6 { get; set; }
        public string f7 { get; set; }
        public string f8 { get; set; }
        public string f9 { get; set; }
        public string f10 { get; set; }
        public string s1 { get; set; }
        public string s2 { get; set; }
        public string s3 { get; set; }
        public string s4 { get; set; }
        public string s5 { get; set; }
        public string s6 { get; set; }
        public string s7 { get; set; }
        public string s8 { get; set; }
        public string s9 { get; set; }
        public string s10 { get; set; }
        public string sec { get; set; }
        public string dept { get; set; }
    }
}
