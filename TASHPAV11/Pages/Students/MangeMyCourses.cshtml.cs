using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TASHPAV11.Mapping;
using TASHPAV11.Model;
using TASHPAV11.HModel;

namespace TASHPAV11.Pages.Students
{
    public class MangeMyCoursesModel : PageModel
    {
        
        public string active_input { get; set; }
        public string submitNewButton { get; set; }
        public string delete_input { get; set; }
        public string deleteButton { get; set; }
        public string msg { get; set; }
        [BindProperty]
        public CourseWithPersonList List { get; set; }
        [BindProperty]
        public CourseWithPerson course1 { get; set; }
        public string DisplayList { get; set; }
        public string insertMSG { get; set; }
        public string insert_button { get; set; }
        public string deleteMSG { get; set; }
        //public Studentss_Select List { get; set; } = new Studentss_Select();
        //public Student_Select Student { get; set; } = new Student_Select();
        [BindProperty]
        public Coursess courseList { get; set; } = new Coursess();
        

        //[BindProperty]
        //public int CourseToAdd { get; set; }
        [BindProperty]
        public string studentName { get; set; }

        public void OnGet()
        {
            active_input = "display:none";
            submitNewButton = "display:none";
            DisplayList = "display:block";
            delete_input = "display:none";
            deleteButton = "display:none";
            studentName = (HttpContext.Session.GetString("Username")).ToString();
            int studentId = int.Parse((HttpContext.Session.GetString("PersonId")).ToString());
            StudentsDB db = new StudentsDB();
            List = db.StudentSelectAll(studentId, true);


        }
        public void OnPostRenderCourses()
        {
            active_input = "display:none";
        submitNewButton = "display:none";
            DisplayList = "display:block";
            delete_input = "display:none";
            deleteButton = "display:none";
            studentName = (HttpContext.Session.GetString("Username")).ToString();
        int studentId = int.Parse((HttpContext.Session.GetString("PersonId")).ToString());
        StudentsDB db = new StudentsDB();
        List = db.StudentSelectAll(studentId,true);
        }

        public void OnPostShowAddCourses()
        {
            DisplayList = "display:none";
            active_input = "display:block";
            insert_button = "display:block";
            delete_input = "display:none";
            deleteButton = "display:none";
            int studentId = int.Parse((HttpContext.Session.GetString("PersonId")).ToString());
            StudentsDB db = new StudentsDB();
            List = db.StudentSelectAll(studentId, false);
        }
        public void OnPostShowDeleteCourse()
        {
            DisplayList = "display:none";
            active_input = "display:none";
            insert_button = "display:none";
            delete_input = "display:block";
            deleteButton = "display:block";
        }

        public void OnPostInsertCourse(int CourseToAdd)
        {

            int studentId = int.Parse((HttpContext.Session.GetString("PersonId")).ToString());
            StudentsDB db = new StudentsDB();
            int records = db.Insert(studentId, CourseToAdd);
            if (records == 1)
            {
                OnPostRenderCourses();
            }
            else { insertMSG = "Could Not Add Course !!!"; }

            //public void Insert(int Insert, int courseID)
            //{


            //    StudentsDB db = new StudentsDB();
            //    int records = db.Insert(Insert, courseID);
            //    delete_input = "display:none";
            //    deleteButton = "display:none";
            //    if (records == 1)
            //    {
            //        insert_button = "disply:none";
            //        insertMSG = "Course Added Successfuly";
            //    }
            //    else { insertMSG = "Could Not Add Course !!!"; }
            //    ;
            //}
        }
    }
}


