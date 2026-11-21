using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TASHPAV11.Mapping;


namespace TASHPAV11.Pages.Courses
{
    public class ManageMarksModel : PageModel
    {

        public string active_input { get; set; }
        public string submitNewButton { get; set; }
        public string show_input { get; set; }
        public string show_button { get; set; }       
        [BindProperty]
        public string display_list { get; set; }
        [BindProperty]
        public string show_students { get; set; }
        
        [BindProperty]
        public string display_student { get; set; }

        [BindProperty]
        public CourseWithPersonList courseList { get; set; } = new CourseWithPersonList();
        int teacherId { get; set; }


        //[BindProperty]
        //public int CourseToAdd { get; set; }

        public string teacherName { get; set; }
        public void OnGet()
        {
            
            OnPostRenderCourses();
        }
        public void OnPostRenderCourses()
        {
            active_input = "display:block";
            submitNewButton = "display:none";
            display_list = "display:block";
            show_input = "display:none";
            show_students = "display:none";
            show_button = "display:none";
            display_student = "display:none";
            teacherName = (HttpContext.Session.GetString("Username")).ToString();
            teacherId = int.Parse((HttpContext.Session.GetString("PersonId")).ToString());
            MarksDB db = new MarksDB();
            // Select all by teacherId, if courseId is 0, it will select all courses for that teacher
            courseList = db.SelectAllByTeacher(teacherId, 0);
        }
        
        public void OnPostShowCourseByTeacher()
        {
            active_input = "display:none";
            submitNewButton = "display:none";
            display_list = "display:none";
            show_input = "display:none";
            show_students = "display:block";
           show_button = "display:block";
            display_student = "display:none";
            teacherName = (HttpContext.Session.GetString("Username")).ToString();
            teacherId = int.Parse( (HttpContext.Session.GetString("PersonId")).ToString());

            MarksDB db = new MarksDB();
            // Select all by teacherId, if courseId is 0, it will select all courses for that teacher
            courseList = db.SelectAllByTeacher(teacherId, 0);
        }
        public void OnPostRenderStudentsBycourse(int CourseToShow)
        {
            active_input = "display:none";
            submitNewButton = "display:none";
            display_list = "display:none";
            show_input = "display:none";
            show_students = "display:block";
            show_button = "display:none";
            display_student = "display:block";
            teacherName = (HttpContext.Session.GetString("Username")).ToString();
            int teacherId = int.Parse((HttpContext.Session.GetString("PersonId")).ToString());
            MarksDB db = new MarksDB();
            // Select Sudent by teacherId and courseId 
            courseList = db.SelectStudentsByCourse(teacherId, CourseToShow);
        }
    }
}
