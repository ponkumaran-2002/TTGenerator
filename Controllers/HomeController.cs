using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using System.Drawing.Printing;
using TTGenerator.Data;
using TTGenerator.Models;
using TTGenerator.Models.Domain;
using System.Reflection.Emit;
using NuGet.Protocol;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Dynamic;
using static System.Collections.Specialized.BitVector32;
using System.Numerics;
using System.Globalization;
using MessagePack.Formatters;

namespace TTGenerator.Controllers
{
    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;

        /*     public HomeController(ILogger<HomeController> logger)
             {
                 _logger = logger;
             }*/
        private static string _Myglobalfid;
        
        public List<clg_tt_level> opted = new List<clg_tt_level>();
        List<View_tt> view = new List<View_tt>();
        Add_faculty? af = new Add_faculty();
        Allot_lab? lab = new Allot_lab();
        public static int glsemester_id;
        public static string glyear_s;
        public static int glbatch_start_year;
        public static string gldept;
        public static string glsec;
        //MVCDemoDbContext
        private readonly MVCDemoDbContext mvcDemoDbContext;
        public HomeController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }
        public IActionResult Clgtt_coordinator_home()
        {
            return View();
        }
        
        public IActionResult Dept_coordinator_home()
        {
            return View();
        }
        public IActionResult Add_course_details()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add_course_details(Add_course_details acd)
        {
            Course_details cd = new Course_details
            {
                Course_id = acd.Course_id,
                Course_name = acd.Course_name,
                Department = acd.Department,
                L = acd.L,
                T = acd.T,
                P = acd.P,
                C = acd.C,
                Course_type = acd.Course_type,
                semester_id=acd.semester_id
            };
            mvcDemoDbContext.Course_details.Add(cd);
            mvcDemoDbContext.SaveChanges();

            return View();
        }
        public IActionResult View_tt()
        {
           // dynamic mymodel = new ExpandoObject();
            //mymodel.View_tt = opted();
            //mymodel.view = view();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> View_tt(View_tt viewtt)
        {
            Console.WriteLine("viewtt.batch_start_year");
            Console.WriteLine(viewtt.batch_start_year);
            Console.WriteLine("viewtt.year_s");
            Console.WriteLine(viewtt.year_s);
            Console.WriteLine("viewtt.semester_id");
            Console.WriteLine(viewtt.semester_id);
            glsemester_id = viewtt.semester_id;
            glyear_s = viewtt.year_s;
            glbatch_start_year=viewtt.batch_start_year;
            return RedirectToAction("View_tt_redirect");
        }
        public void FetchData()
        {

            var clg = mvcDemoDbContext.clg_tt_level.FromSqlRaw($"select * from [clg_tt_level] where semester_id = {glsemester_id} and year_s = '{glyear_s}' and batch_start_year = {glbatch_start_year}").ToList();
            //where semester_id = { viewtt.semester_id } and year_s = '{viewtt.year_s}' and batch_start_year = { viewtt.batch_start_year }" 
            Console.WriteLine(clg[0].elective_id);
            Console.WriteLine(clg.Count);
            int i = 0;
            int c = clg.Count;
            Console.WriteLine("c-count");
            Console.WriteLine(c);
            while (i < c)
            {
                opted.Add(new clg_tt_level()
                {
                    sno = clg[i].sno,
                    semester_id = clg[i].semester_id,
                    year_s = clg[i].year_s,
                    batch_start_year = clg[i].batch_start_year,
                    slot = clg[i].slot,
                    day_tt = clg[i].day_tt,
                    elective_id = clg[i].elective_id
                });
                i++;
            }
            Console.WriteLine("opted");
            Console.WriteLine(opted.Count);
            //Console.WriteLine();
            Console.WriteLine(opted[0].sno);

        }
        public IActionResult View_tt_redirect()
        {
            // var opt=opted.ToList();
            //Console.WriteLine(opt[0].sno);
          //  Console.WriteLine(opted[0].sno);
            FetchData();
            Console.WriteLine(opted[0].sno);
            return View(opted);
        }

        public IActionResult Add_faculty()
        {
           Console.WriteLine(_Myglobalfid);
       //    var fid = mvcDemoDbContext.Login_credentials.FromSqlRaw($"select * from [Login_credentials] where  login_id ='{_Mygloballid}'").ToList();
           //Console.WriteLine(fid[0].faculty_id);
           var fac = mvcDemoDbContext.Faculty_details.FromSqlRaw($"select * from [Faculty_details] where  faculty_id ='{_Myglobalfid}'").ToList();
           Console.WriteLine(fac[0].faculty_dept);
           var fac2 = mvcDemoDbContext.Faculty_details.FromSqlRaw($"select * from [Faculty_details] where  faculty_dept ='{fac[0].faculty_dept}'").ToList();
           var cd1 = mvcDemoDbContext.Course_details.FromSqlRaw($"select * from [Course_details] where  Department ='{fac[0].faculty_dept}'").ToList();
            Console.WriteLine(fac2.Count);
            //var dropdownvd = new SelectList(fac2, "faculty_id", "faculty_name");
         //  var listfname=new List<string>();
           //var listfid = new List<string>();
           List<SelectListItem> faculty_name_id = new List<SelectListItem>();
           List<SelectListItem> course_name_id=new List<SelectListItem>();
           // faculty_name_id.Add(new SelectListItem() { Text = "Northern Cape", Value = "NC" });
           // faculty_name_id.Add(new SelectListItem() { Text = "Free State", Value = "FS" });
           // faculty_name_id.Add(new SelectListItem() { Text = "Western Cape", Value = "WC" });
          
           // Add_course_details ac = new Add_course_details();
            af.FacultyCollection = new List<Faculty>();
            af.CourseCollection = new List<Course>();

            for (int i=0;i < fac2.Count;i++)
            {
               faculty_name_id.Add(new SelectListItem() { Text = fac2[i].faculty_name, Value = fac2[i].faculty_id });
                af.FacultyCollection.Add(new Faculty() { Faculty_Id = fac2[i].faculty_id, Faculty_Name = fac2[i].faculty_name });

            }
            for (int i=0;i<cd1.Count;i++)
            {
                course_name_id.Add(new SelectListItem() { Text = cd1[i].Course_name, Value = cd1[i].Course_id });
                af.CourseCollection.Add(new Course() { Course_Id = cd1[i].Course_id, Course_Name = cd1[i].Course_name });
            }
            
            ViewData["fni"] = faculty_name_id;
            ViewData["cni"] = course_name_id;
            ViewBag.facname=faculty_name_id;
            ViewBag.course = course_name_id;
            return View(af);
        }
        [HttpPost]
        public async Task<IActionResult>Add_faculty(Add_faculty add_faculty)
        {
            Console.WriteLine(add_faculty.Faculty_id);
            var fac = mvcDemoDbContext.Faculty_details.FromSqlRaw($"select * from [Faculty_details] where  faculty_id ='{_Myglobalfid}'").ToList();

            allot_faculty allotfac = new allot_faculty
            {
                     Faculty_id = add_faculty.Faculty_id,
                     Course_id =  add_faculty.Course_id,
                     S_year = add_faculty.Year,
                     Batch = add_faculty.Batch,
                     Section=add_faculty.section,
                     Semester=add_faculty.Semester,
                department = fac[0].faculty_dept

    };
            mvcDemoDbContext.allot_faculty.Add(allotfac);
            mvcDemoDbContext.SaveChanges();
            return RedirectToAction("Add_faculty");
        }
        public IActionResult allotlab()
        {
           
            return View(lab);
        }
        [HttpPost]
        public async Task<IActionResult>allotlab(Allot_lab allab)
        {
            glsemester_id = allab.semester_id;
            glyear_s = allab.year_s;
            glbatch_start_year = allab.batch_start_year;
            gldept = allab.dept;
            glsec = allab.sec;
            Console.WriteLine("glbatch");
            Console.WriteLine(glbatch_start_year);
            return RedirectToAction("allotlab_redirect");
        }
        public void FetchData1()
        {

            var clg = mvcDemoDbContext.clg_tt_level.FromSqlRaw($"select * from [clg_tt_level] where semester_id = {glsemester_id} and year_s = '{glyear_s}' and batch_start_year = {glbatch_start_year}").ToList();
            int i = 0;
            int c = clg.Count;
            Console.WriteLine("c-count");
            Console.WriteLine(c);
            lab.Opted_electives = new List<clg_tt_level>();
            while (i < c)
            {
                lab.Opted_electives.Add(new clg_tt_level()
                {
                    sno = clg[i].sno,
                    semester_id = clg[i].semester_id,
                    year_s = clg[i].year_s,
                    batch_start_year = clg[i].batch_start_year,
                    slot = clg[i].slot,
                    day_tt = clg[i].day_tt,
                    elective_id = clg[i].elective_id
                });
                i++;
            }
            Console.WriteLine("opted");
            Console.WriteLine(lab.Opted_electives.Count);
            Console.WriteLine(lab.Opted_electives[0].sno);

        }
        public IActionResult generate_initial()
        {
            return View();
        }
        public List<List<string>> retrive_data(string s_year,int batch_start_year,string section,int semester_id)
        {
            var fac = mvcDemoDbContext.Faculty_details.FromSqlRaw($"select * from [Faculty_details] where  faculty_id ='{_Myglobalfid}'").ToList();
            var allot_fac = mvcDemoDbContext.allot_faculty.FromSqlRaw($"select * from [allot_faculty] where S_Year= '{s_year}' and Batch={batch_start_year} and section='{section}' and semester={semester_id} and department='{fac[0].faculty_dept}'").ToList();
            var allot_lab = mvcDemoDbContext.allot_lab.FromSqlRaw($"select * from [allot_lab] where dept='{fac[0].faculty_dept}' and sec='{section}' and semester_id={semester_id} and year_s='{s_year}' and batch_start_year={batch_start_year}").ToList();
            var clg_tt_level = mvcDemoDbContext.clg_tt_level.FromSqlRaw($"select * from [clg_tt_level] where batch_start_year={batch_start_year} and semester_id={semester_id} and year_s='{s_year}'").ToList();
            var tt = mvcDemoDbContext.tt.FromSqlRaw($"select * from [tt] where department='{fac[0].faculty_dept}'");
            List<List<string>> tt_temp = new List<List<string>>();
            List<string> inner_tt1 = new List<string>();
            List<string> inner_tt2 = new List<string>();
            List<string> inner_tt3 = new List<string>();
            List<string> inner_tt4 = new List<string>();
            List<string> inner_tt5 = new List<string>();
            List<string> inner_tt6 = new List<string>();
            //Console.WriteLine(clg_tt_level.Count());
            if (clg_tt_level != null)
            {
                for (int j = 0; j < clg_tt_level.Count(); j++)
                {
                    //Console.WriteLine(j);
                    if (j >= 0 && j < 10)
                    {
                        if (clg_tt_level[j].elective_id != 106)
                        {
                            inner_tt1.Add(clg_tt_level[j].elective_id.ToString());
                        }
                        else
                        {
                            inner_tt1.Add("NA");
                        }
                    }
                    else if (j >= 10 && j < 20)
                    {
                        if (clg_tt_level[j].elective_id != 106)
                        {
                            inner_tt2.Add(clg_tt_level[j].elective_id.ToString());
                        }
                        else
                        {
                            inner_tt2.Add("NA");
                        }
                    }
                    else if (j >= 20 && j < 30)
                    {
                        if (clg_tt_level[j].elective_id != 106)
                        {
                            inner_tt3.Add(clg_tt_level[j].elective_id.ToString());
                        }
                        else
                        {
                            inner_tt3.Add("NA");
                        }
                    }
                    else if (j >= 30 && j < 40)
                    {
                        if (clg_tt_level[j].elective_id != 106)
                        {
                            inner_tt4.Add(clg_tt_level[j].elective_id.ToString());
                        }
                        else
                        {
                            inner_tt4.Add("NA");
                        }
                    }
                    else if (j >= 40 && j < 50)
                    {
                        if (clg_tt_level[j].elective_id != 106)
                        {
                            inner_tt5.Add(clg_tt_level[j].elective_id.ToString());
                        }
                        else
                        {
                            inner_tt5.Add("NA");
                        }
                    }
                    else
                    {
                        if (clg_tt_level[j].elective_id != 106)
                        {
                            inner_tt6.Add(clg_tt_level[j].elective_id.ToString());
                        }
                        else
                        {
                            inner_tt6.Add("NA");
                        }
                    }

                }
            }
            tt_temp.Add(inner_tt1);
            tt_temp.Add(inner_tt2);
            tt_temp.Add(inner_tt3);
            tt_temp.Add(inner_tt4);
            tt_temp.Add(inner_tt5);
            tt_temp.Add(inner_tt6);

            for (int i = 0; i < allot_lab.Count(); i++)
            {

                if (allot_lab[i].slot == 1 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][0] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 2 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][1] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 3 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][2] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 4 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][3] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 5 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][4] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 6 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][5] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 7 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][6] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 8 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][7] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 9 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][8] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 10 && allot_lab[i].day_tt == "monday")
                {
                    tt_temp[0][9] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 1 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][0] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 2 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][1] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 3 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][2] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 4 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][3] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 5 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][4] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 6 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][5] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 7 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][6] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 8 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][7] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 9 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][8] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 10 && allot_lab[i].day_tt == "tuesday")
                {
                    tt_temp[1][9] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 1 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][0] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 2 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][1] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 3 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][2] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 4 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][3] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 5 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][4] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 6 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][5] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 7 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][6] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 8 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][7] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 9 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][8] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 10 && allot_lab[i].day_tt == "wednesday")
                {
                    tt_temp[2][9] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 1 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][0] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 2 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][1] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 3 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][2] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 4 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][3] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 5 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][4] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 6 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][5] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 7 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][6] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 8 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][7] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 9 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][8] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 10 && allot_lab[i].day_tt == "thursday")
                {
                    tt_temp[3][9] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 1 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][0] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 2 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][1] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 3 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][2] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 4 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][3] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 5 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][4] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 6 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][5] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 7 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][6] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 8 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][7] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 9 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][8] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 10 && allot_lab[i].day_tt == "friday")
                {
                    tt_temp[4][9] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 1 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][0] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 2 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][1] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 3 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][2] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 4 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][3] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 5 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][4] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 6 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][5] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 7 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][6] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 8 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][7] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 9 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][8] = allot_lab[i].course_id;
                }
                if (allot_lab[i].slot == 10 && allot_lab[i].day_tt == "saturday")
                {
                    tt_temp[5][9] = allot_lab[i].course_id;
                }
            }
            //Console.WriteLine(tt_temp[0][8]);
            Console.WriteLine(allot_fac.Count());
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(tt_temp[i][j] + '\t');
                }
                Console.WriteLine();
            }
            return tt_temp;
        }
        public bool check_consecutive_occurence(List<string> numbers,List<string>mat0)
        {
            //List<int> numbers = new List<int> { 1, 2, 2, 3, 4, 4, 4, 5, 6, 6 };

            string previous_str = null;
            int consecutiveCount = 0;

            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (mat0.Contains(numbers[i])!=true )
                {
                    if (numbers[i]==previous_str)
                    {
                        consecutiveCount++;
                    }
                    else
                    {
                        consecutiveCount = 1;
                    }
                    previous_str = numbers[i];
                    if (consecutiveCount >= 2)
                    {
                        Console.WriteLine("Cons Occurs AT" + i);
                        return true;
                    }
                }
            }
            return false;

        }
       public bool check_period_collision_occur(List<List<string>> mat1,List<List<tt>> mat2)
        {
            for(int i=0;i<mat2.Count;i++)
            {   List<List<string>> st1 = new List<List<string>>();
                int l = 0; 
                for(int j=0;j<6;j++)
                {   List<string> st2=new List<string>();
                    for(int k=0;k<10;k++)
                    {   if(l>=60)
                        {
                            l = 0;
                        }
                        st2.Add(mat2[i][l].faculty_id.ToString());
                    }
                    st1.Add(st2);
                }
                for(int j=0;j<6;j++)
                {
                    for(int k=0;k<10;k++)
                    {
                        if (mat1[j][k]!="102"|| mat1[j][k] != "101" || mat1[j][k] != "104" || mat1[j][k] != "107" || mat1[j][k] != "103" || mat1[j][k] != "ss" || mat1[j][k] != "CE-1")
                        {
                            if (mat1[j][k] == st1[j][k])
                            {
                                return true;
                                break;
                            }
                        }

                    }
                }
            }
            return false;
        }
        public bool Check(List<List<string>> mat1,List<string>ommited_str,List<List<string>>mat2,List<List<tt>> mat3)
        {
            int flag = 1;
            for(int i=0;i<6;i++)
            {
                if (check_consecutive_occurence(mat1[i], ommited_str) == true)
                {
                    Console.WriteLine("Checking Consecutive occurence"+i);
                    
                    return true;
                }
            }
            if(check_period_collision_occur(mat2,mat3)==true)
            {
                    return true;
            }
            return false;

        }
       public List<List<string>>shuffle(List<List<string>> mat,List<string>mat0)
       {
            List<string> t1 = new List<string>();
            List<int> fixedElements = new List<int>();
            //List<int> shuffledElements = new List<int>();
            for (int i = 0;i<6;i++)
            {
                for(int j=0;j<10;j++)
                {
                    if (mat0.Contains(mat[i][j])!=true)
                    {
                        t1.Add(mat[i][j]);
                    }
                }
            }
            Random random = new Random();
            int n = t1.Count;
            while(n>1)
            {
                n--;
                int k=random.Next(n+1);
                string value = t1[k];
                t1[k] = t1[n];
                t1[n] = value;
            }
            int coun = 0;
            for(int i=0;i<6;i++)
            {
                for(int j=0;j<10;j++)
                {  if(coun<t1.Count)
                    {
                        if (mat0.Contains(mat[i][j]) != true)
                        {
                            mat[i][j] = t1[coun];
                            coun++;
                        }

                    }

                }
            }
            return mat;
        }
        public string get_facultyid(string cid,List<allot_faculty>af)
        {
           string fac_id=cid;
           for(int i=0;i<af.Count;i++)
           {
               if (af[i].Course_id!="107" && af[i].Course_id!="101" && af[i].Course_id!="102" && af[i].Course_id!="104" && af[i].Course_id!="ss" && af[i].Course_id!="103")
                {
                    if (af[i].Course_id==cid)
                    {
                        fac_id = af[i].Faculty_id;
                    }
                }
           }
            return fac_id;
        }
        public string get_fc(string cid,List<allot_faculty>af)
        {    string fc_id=cid;
            for(int i=0;i<af.Count;i++)
            {
                if (af[i].Course_id != "107" && af[i].Course_id != "101" && af[i].Course_id != "102" && af[i].Course_id != "104" && af[i].Course_id != "ss" && af[i].Course_id != "103")
                {
                    if (af[i].Course_id == cid)
                    {
                        fc_id = af[i].Faculty_id;
                        break;
                    }
                }

            }

            return fc_id;
        }
        [HttpPost]
        public async Task<IActionResult> generate_initial(generate_initial gl)
        {   
            Console.WriteLine(gl.s_year);
            Console.WriteLine(gl.section);
            List<List<string>> te= new List<List<string>>();
            te=retrive_data(gl.s_year, gl.batch_start_year, gl.section, gl.semester_id);
            var fac = mvcDemoDbContext.Faculty_details.FromSqlRaw($"select * from [Faculty_details] where  faculty_id ='{_Myglobalfid}'").ToList();
            var ret = mvcDemoDbContext.tt.FromSqlRaw($"select * from [tt] where department='{fac[0].faculty_dept}'");
            List<string> n_years = new List<string>() { "I", "II", "III" , "IV" };
            List<int> n_sections = new List<int>();
            var ns = mvcDemoDbContext.tt.FromSqlRaw($"select * from [tt] where department='{fac[0].faculty_dept}'").ToList();
            var af1 = mvcDemoDbContext.allot_faculty.FromSqlRaw($"select Distinct * from [allot_faculty] where S_Year= '{gl.s_year}' and Batch={gl.batch_start_year} and Section='{gl.section}' and Semester={gl.semester_id} and Department='{fac[0].faculty_dept}'").ToList();
            List<string>ommitingsubs= new List<string>();
            var c_details = mvcDemoDbContext.Course_details.FromSqlRaw($"select * from [Course_details] where  P>0 or Course_type= 102 or Course_type=108 or course_type=101 or course_type=103 and Department ='{fac[0].faculty_dept}' ").ToList();
            Console.WriteLine("all_facccccc" + af1.Count);
            List<List<string>> tfa = new List<List<string>>();          
            List<List<tt>> groups=new List<List<tt>>();
            for(int i=0;i<ns.Count;i+=60)
            {
                List<tt> group = ns.GetRange(i, Math.Min(60, ns.Count - i));
                groups.Add(group);
            }         
         for(int i=0;i<c_details.Count;i++)
            {
                ommitingsubs.Add(c_details[i].Course_id);
            }
            ommitingsubs.Add("102");
            ommitingsubs.Add("101");
            ommitingsubs.Add("103");
            ommitingsubs.Add("104");
            ommitingsubs.Add("105");
            ommitingsubs.Add("106");
            ommitingsubs.Add("107");
            ommitingsubs.Add("108");
            Console.WriteLine("all_fac" + af1[0].Course_id);
            int lvo = 0;
            for(int i=0;i<6;i++)
            {
                for(int j=0;j<10;j++)
                {
                    if (te[i][j]=="NA")
                    {
                        lvo++;
                    }
                }
            }
            Console.WriteLine("LVOOO" + lvo);
            for(int i=0;i<af1.Count;i++)
            {   
                Console.WriteLine(af1[i].Course_id);
            }    
            if(ret.Count()==0)
            {
                Console.WriteLine("Inside if statement");
                int k = 0;
                for (int i = 0; i < 6; i++)
                {
                        for (int j = 0; j < 10; j++)
                    {
                        if (te[i][j]=="NA")
                        {   
                            while(true)
                            { if(k>=af1.Count)
                                {
                                    k = 0;
                                }
                                if (ommitingsubs.Contains(af1[k].Course_id))
                                {
                                    k++;
                                }
                                else
                                {
                                    te[i][j] = af1[k].Course_id;
                                    k++;
                                    break;
                                }
                            }

                        }

                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.Write(te[i][j] + '\t');
                    }
                    Console.WriteLine();
                }
                for (int i = 0; i < ommitingsubs.Count; i++)
                {
                    Console.WriteLine(ommitingsubs[i]);
                }
                while (true)
                {
                    if (Check(te,ommitingsubs,tfa,groups) == true)
                    {
                        shuffle(te, ommitingsubs);
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (i == 0)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "monday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();

                        }
                        else if (i == 1)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "tuesday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                        else if (i == 2)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "wednesday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                        else if (i == 3)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "thursday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                        else if (i == 4)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "friday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                        else
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "saturday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.WriteLine(te[i][j] + '\t');
                    }
                    Console.WriteLine('\n');
                }
            }
            else {

                int k = 0;
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (te[i][j] == "NA")
                        {
                            while (true)
                            {
                                if (k >= af1.Count)
                                {
                                    k = 0;
                                }
                                if (ommitingsubs.Contains(af1[k].Course_id))
                                {
                                    k++;
                                }
                                else
                                {
                                    te[i][j] = af1[k].Course_id;
                                    k++;
                                    break;
                                }
                            }

                        }

                    }
                }
                Console.WriteLine(te.Count);
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.Write(te[i][j] + '\t');
                    }
                    Console.WriteLine();
                }
                for (int i = 0; i < 6; i++)
                {   List<string> kgf= new List<string>();
                    for (int j = 0; j < 10; j++)
                    {
                        kgf.Add(te[i][j].ToString());
                    }
                    tfa.Add(kgf);
                }
                
                for (int i = 0; i < ommitingsubs.Count; i++)
                {
                    Console.WriteLine(ommitingsubs[i]);
                }
                while (true)
                {
                    if (Check(te, ommitingsubs, tfa, groups) == true)
                    {
                        shuffle(te, ommitingsubs);
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (i == 0)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "monday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();

                        }
                        else if (i == 1)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "tuesday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                        else if (i == 2)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "wednesday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                        else if (i == 3)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "thursday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                        else if (i == 4)
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "friday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                        else
                        {
                            int c = mvcDemoDbContext.tt.Count();
                            Console.WriteLine(c);
                            tt clg = new tt
                            {
                                sno = c + 1,
                                semester_id = gl.semester_id,
                                batch_start_year = gl.batch_start_year,
                                year_s = gl.s_year,
                                slot = j + 1,
                                day_tt = "saturday",
                                course_id = te[i][j],
                                department = fac[0].faculty_dept,
                                section = gl.section,
                                faculty_id = get_facultyid(te[i][j], af1)
                            };
                            mvcDemoDbContext.tt.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.WriteLine(te[i][j] + '\t');
                    }
                    Console.WriteLine('\n');
                }
            }

            

            return View();
        }
        public IActionResult allotlab_redirect()
        {
            var fac = mvcDemoDbContext.Faculty_details.FromSqlRaw($"select * from [Faculty_details] where  faculty_id ='{_Myglobalfid}'").ToList();
            var c_details = mvcDemoDbContext.Course_details.FromSqlRaw($"select * from [Course_details] where  P>0 and Department ='{fac[0].faculty_dept}' ").ToList();
            List<SelectListItem> course_name_id = new List<SelectListItem>();
            lab.CourseCollection = new List<Lab>();
            for (int i = 0; i < c_details.Count; i++)
            {
                course_name_id.Add(new SelectListItem() { Text = c_details[i].Course_name, Value = c_details[i].Course_id });
                lab.CourseCollection.Add(new Lab() { Course_Id = c_details[i].Course_id, Course_Name = c_details[i].Course_name });

            }
            FetchData1();

            return View(lab); 
        
        }
        [HttpPost]
        public async Task<IActionResult> allotlab_redirect(Allot_lab index)
        {
            index.sec = glsec;
            index.dept = gldept;
            index.semester_id = glsemester_id;
            index.batch_start_year = glbatch_start_year;
            index.year_s = glyear_s;
            Console.WriteLine("index.m1");
            Console.WriteLine(index.m1);
                        if (index.m1 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                //Console.WriteLine(index.m1);
                            allot_lab clg = new allot_lab
                            {   sno=c+1,
                                dept = index.dept,
                                sec = index.sec,
                                semester_id = index.semester_id,
                                batch_start_year = index.batch_start_year,
                                year_s = index.year_s,
                                slot = 1,
                                day_tt = "monday",
                                course_id = index.m1
                            };
                            mvcDemoDbContext.allot_lab.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                    
                        if (index.m2 != null)
                        {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                            {   sno=c+1,
                                dept = index.dept,
                                sec = index.sec,
                                semester_id = index.semester_id,
                                batch_start_year = index.batch_start_year,
                                year_s = index.year_s,
                                slot = 2,
                                day_tt = "monday",
                                course_id = index.m2
                            };
                            mvcDemoDbContext.allot_lab.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
                    
                        if (index.m3 != null)
                        {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                            {   sno=c+1,
                                dept = index.dept,
                                sec = index.sec,
                                semester_id = index.semester_id,
                                batch_start_year = index.batch_start_year,
                                year_s = index.year_s,
                                slot = 3,
                                day_tt = "monday",
                                course_id = index.m3
                            };
                            mvcDemoDbContext.allot_lab.Add(clg);
                            mvcDemoDbContext.SaveChanges();
                        }
            if (index.m4 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 4,
                    day_tt = "monday",
                    course_id = index.m4
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.m5 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 5,
                    day_tt = "monday",
                    course_id = index.m5
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.m6 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 6,
                    day_tt = "monday",
                    course_id = index.m6
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.m7 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {  sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 7,
                    day_tt = "monday",
                    course_id = index.m7
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
             if (index.m8 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 8,
                    day_tt = "monday",
                    course_id = index.m8
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.m9 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 9,
                    day_tt = "monday",
                    course_id = index.m9
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.m10 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 10,
                    day_tt = "monday",
                    course_id = index.m10
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t1 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 1,
                    day_tt = "tuesday",
                    course_id = index.t1
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t2 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 2,
                    day_tt = "tuesday",
                    course_id = index.t2
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t3 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 3,
                    day_tt = "tuesday",
                    course_id = index.t3
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t4 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 4,
                    day_tt = "tuesday",
                    course_id = index.t4
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t5 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 5,
                    day_tt = "tuesday",
                    course_id = index.t5
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t6 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 6,
                    day_tt = "tuesday",
                    course_id = index.t6
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t7 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 7,
                    day_tt = "tuesday",
                    course_id = index.t7
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t8 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 8,
                    day_tt = "tuesday",
                    course_id = index.t8
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t9 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 9,
                    day_tt = "tuesday",
                    course_id = index.t9
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.t10 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 10,
                    day_tt = "tuesday",
                    course_id = index.t10
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.w1 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {  sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 1,
                    day_tt = "wednesday",
                    course_id = index.w1
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.w2 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {  sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 2,
                    day_tt = "wednesday",
                    course_id = index.w2
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.w3 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 3,
                    day_tt = "wednesday",
                    course_id = index.w3
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.w4 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 4,
                    day_tt = "wednesday",
                    course_id = index.w4
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
           if (index.w5 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 5,
                    day_tt = "wednesday",
                    course_id = index.w5
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.w6 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 6,
                    day_tt = "wednesday",
                    course_id = index.w6
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.w7 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 7,
                    day_tt = "wednesday",
                    course_id = index.w7
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
           if (index.w8 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 8,
                    day_tt = "wednesday",
                    course_id = index.w8
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
           if (index.w9 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 9,
                    day_tt = "wednesday",
                    course_id = index.w9
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.w10 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 10,
                    day_tt = "wednesday",
                    course_id = index.w10
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th1 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 1,
                    day_tt = "thursday",
                    course_id = index.th1
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th2 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 2,
                    day_tt = "thursday",
                    course_id = index.th2
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th3 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 3,
                    day_tt = "thursday",
                    course_id = index.th3
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th4 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 4,
                    day_tt = "thursday",
                    course_id = index.th4
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th5 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 5,
                    day_tt = "thursday",
                    course_id = index.th5
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th6 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 6,
                    day_tt = "thursday",
                    course_id = index.th6
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th7 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 7,
                    day_tt = "thursday",
                    course_id = index.th7
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th8 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 8,
                    day_tt = "thursday",
                    course_id = index.th8
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th9 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 9,
                    day_tt = "thursday",
                    course_id = index.th9
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.th10 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 10,
                    day_tt = "thursday",
                    course_id = index.th10
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f1 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 1,
                    day_tt = "friday",
                    course_id = index.f1
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f2 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 2,
                    day_tt = "friday",
                    course_id = index.f2
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f3 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 3,
                    day_tt = "friday",
                    course_id = index.f3
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f4 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 4,
                    day_tt = "friday",
                    course_id = index.f4
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f5 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 5,
                    day_tt = "friday",
                    course_id = index.f5
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f6 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 6,
                    day_tt = "friday",
                    course_id = index.f6
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f7 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 7,
                    day_tt = "friday",
                    course_id = index.f7
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f8 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 8,
                    day_tt = "friday",
                    course_id = index.f8
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f9 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 9,
                    day_tt = "friday",
                    course_id = index.f9
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.f10 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 10,
                    day_tt = "friday",
                    course_id = index.f10
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.s1 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {  sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 1,
                    day_tt = "saturday",
                    course_id = index.s1
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.s2 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {  sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 2,
                    day_tt = "saturday",
                    course_id = index.s2
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.s3 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 3,
                    day_tt = "saturday",
                    course_id = index.s3
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.s4 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 4,
                    day_tt = "saturday",
                    course_id = index.s4
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.s5 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 5,
                    day_tt = "saturday",
                    course_id = index.s5
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.s6 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 6,
                    day_tt = "saturday",
                    course_id = index.s6
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }

            if (index.s7 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 7,
                    day_tt = "saturday",
                    course_id = index.s7
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.s8 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 8,
                    day_tt = "saturday",
                    course_id = index.s8
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.s9 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 9,
                    day_tt = "saturday",
                    course_id = index.s9
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }
            if (index.s10 != null)
            {
                int c = mvcDemoDbContext.allot_lab.Count();
                allot_lab clg = new allot_lab
                {   sno=c+1,
                    dept = index.dept,
                    sec = index.sec,
                    semester_id = index.semester_id,
                    batch_start_year = index.batch_start_year,
                    year_s = index.year_s,
                    slot = 10,
                    day_tt = "saturday",
                    course_id = index.s10
                };
                mvcDemoDbContext.allot_lab.Add(clg);
                mvcDemoDbContext.SaveChanges();
            }

            return RedirectToAction("allotlab");
        }
        public IActionResult Index1()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index1(Index1 index)
        {
            Console.WriteLine("index.semester_id");
            Console.WriteLine(index.semester_id);
            Console.WriteLine(index.m10);
            Console.WriteLine(index.m1);            
            for (int i = 0; i < 6; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    if ((i == 0)&&(j==0))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg=new clg_tt_level { sno=c+1,
                                                           semester_id= index.semester_id ,
                                                           batch_start_year=index.batch_start_year,
                                                           year_s = index.year_s,
                                                           slot = 1,
                                                           day_tt="monday",
                                                           elective_id=index.m1
                                                            };
                         mvcDemoDbContext.clg_tt_level.Add(clg);
                         mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 0) && (j == 1))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 2,
                            day_tt = "monday",
                            elective_id = index.m2
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 0) && (j == 2))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 3,
                            day_tt = "monday",
                            elective_id = index.m3
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 0) && (j == 3))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 4,
                            day_tt = "monday",
                            elective_id = index.m4
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 0) && (j == 4))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 5,
                            day_tt = "monday",
                            elective_id = index.m5
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 0) && (j == 5))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 6,
                            day_tt = "monday",
                            elective_id = index.m6
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 0) && (j == 6))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno = c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 7,
                            day_tt = "monday",
                            elective_id = index.m7
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 0) && (j == 7))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 8,
                            day_tt = "monday",
                            elective_id = index.m8
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 0) && (j == 8))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 9,
                            day_tt = "monday",
                            elective_id = index.m9
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 0) && (j == 9))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 10,
                            day_tt = "monday",
                            elective_id = index.m10
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 0))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 1,
                            day_tt = "tuesday",
                            elective_id = index.t1
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 1))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 2,
                            day_tt = "tuesday",
                            elective_id = index.t2
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 2))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 3,
                            day_tt = "tuesday",
                            elective_id = index.t3
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 3))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {  sno= c+1,    
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 4,
                            day_tt = "tuesday",
                            elective_id = index.t4
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 4))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 5,
                            day_tt = "tuesday",
                            elective_id = index.t5
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 5))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {  sno= c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 6,
                            day_tt = "tuesday",
                            elective_id = index.t6
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 6))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {    sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 7,
                            day_tt = "tuesday",
                            elective_id = index.t7
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 7))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {    sno=c+1,   
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 8,
                            day_tt = "tuesday",
                            elective_id = index.t8
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 8))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,    
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 9,
                            day_tt = "tuesday",
                            elective_id = index.t9
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 1) && (j == 9))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno = c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 10,
                            day_tt = "tuesday",
                            elective_id = index.t10
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 0))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {    sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 1,
                            day_tt = "wednesday",
                            elective_id = index.w1
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 1))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        { sno = c + 1,  
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 2,
                            day_tt = "wednesday",
                            elective_id = index.w2
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 2))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno =c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 3,
                            day_tt = "wednesday",
                            elective_id = index.w3
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 3))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 4,
                            day_tt = "wednesday",
                            elective_id = index.w4
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 4))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 5,
                            day_tt = "wednesday",
                            elective_id = index.w5
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 5))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 6,
                            day_tt = "wednesday",
                            elective_id = index.w6
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 6))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno = c+1,  
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 7,
                            day_tt = "wednesday",
                            elective_id = index.w7
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 7))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 8,
                            day_tt = "wednesday",
                            elective_id = index.w8
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 8))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 9,
                            day_tt = "wednesday",
                            elective_id = index.w9
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 2) && (j == 9))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 10,
                            day_tt = "wednesday",
                            elective_id = index.w10
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 0))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        { sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 1,
                            day_tt = "thursday",
                            elective_id = index.th1
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 1))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno = c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 2,
                            day_tt = "thursday",
                            elective_id = index.th2
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 2))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 3,
                            day_tt = "thursday",
                            elective_id = index.th3
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 3))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {    sno= c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 4,
                            day_tt = "thursday",
                            elective_id = index.th4
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 4))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 5,
                            day_tt = "thursday",
                            elective_id = index.th5
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 5))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 6,
                            day_tt = "thursday",
                            elective_id = index.th6
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 6))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {sno = c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 7,
                            day_tt = "thursday",
                            elective_id = index.th7
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 7))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 8,
                            day_tt = "thursday",
                            elective_id = index.th8
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 8))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 9,
                            day_tt = "thursday",
                            elective_id = index.th9
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 3) && (j == 9))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {  sno= c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 10,
                            day_tt = "thursday",
                            elective_id = index.th10
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 0))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno = c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 1,
                            day_tt = "friday",
                            elective_id = index.f1
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 1))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        { sno  = c + 1, 
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 2,
                            day_tt = "friday",
                            elective_id = index.f2
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 2))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno = c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 3,
                            day_tt = "friday",
                            elective_id = index.f3
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 3))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {  sno= c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 4,
                            day_tt = "friday",
                            elective_id = index.f4
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 4))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno= c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 5,
                            day_tt = "friday",
                            elective_id = index.f5
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 5))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 6,
                            day_tt = "friday",
                            elective_id = index.f6
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 6))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 7,
                            day_tt = "friday",
                            elective_id = index.f7
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 7))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 8,
                            day_tt = "friday",
                            elective_id = index.f8
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 8))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 9,
                            day_tt = "friday",
                            elective_id = index.f9
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 4) && (j == 9))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 10,
                            day_tt = "friday",
                            elective_id = index.f10
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 5) && (j == 0))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 1,
                            day_tt = "saturday",
                            elective_id = index.s1
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 5) && (j == 1))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 2,
                            day_tt = "saturday",
                            elective_id = index.s2
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 5) && (j == 2))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 3,
                            day_tt = "saturday",
                            elective_id = index.s3
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 5) && (j == 3))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 4,
                            day_tt = "saturday",
                            elective_id = index.s4
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 5) && (j == 4))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {    sno=c+1,   
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 5,
                            day_tt = "saturday",
                            elective_id = index.s5
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 5) && (j == 5))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 6,
                            day_tt = "saturday",
                            elective_id = index.s6
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 5) && (j == 6))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {   sno=c+1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 7,
                            day_tt = "saturday",
                            elective_id = index.s7
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 5) && (j == 7))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {
                            sno = c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 8,
                            day_tt = "saturday",
                            elective_id = index.s8
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else if ((i == 5) && (j == 8))
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {
                            sno = c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 9,
                            day_tt = "saturday",
                            elective_id = index.s9
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();

                    }
                    else
                    {
                        int c = mvcDemoDbContext.clg_tt_level.Count();
                        Console.WriteLine(c);
                        clg_tt_level clg = new clg_tt_level
                        {
                            sno = c + 1,
                            semester_id = index.semester_id,
                            batch_start_year = index.batch_start_year,
                            year_s = index.year_s,
                            slot = 10,
                            day_tt = "saturday",
                            elective_id = index.s10
                        };
                        mvcDemoDbContext.clg_tt_level.Add(clg);
                        mvcDemoDbContext.SaveChanges();
                    }
                }
            }
            return View();

        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login loginrequest)
        {    Console.WriteLine("loginrequest.login_id");
             Console.WriteLine(loginrequest.login_id);
             var li = loginrequest.login_id.ToString();
             Console.WriteLine(loginrequest.login_id.GetType());
             Console.WriteLine(li.GetType());
             var login = await mvcDemoDbContext.Login_credentials.FindAsync(li);
            _Myglobalfid = login.faculty_id;
            if (login != null)
            {
                
                if((login.password_l==loginrequest.password_l)&&(login.roleid==104))
                {
                    Console.WriteLine(login);
                    Console.WriteLine(login.password_l);
                    Console.WriteLine(login.roleid);
                    Console.WriteLine(login.faculty_id);
                    // return RedirectToAction("Index1");
                    return RedirectToAction("Clgtt_coordinator_home");
                }
                if ((login.password_l == loginrequest.password_l) && (login.roleid == 103))
                {
                    Console.WriteLine(login);
                    Console.WriteLine(login.password_l);
                    Console.WriteLine(login.roleid);
                    Console.WriteLine(login.faculty_id);
                    // return RedirectToAction("Index1");
                    return RedirectToAction("Dept_coordinator_home");
                }
            }
            return RedirectToAction("Error");

        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}