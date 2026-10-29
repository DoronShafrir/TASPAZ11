using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TASHPAV11.HModel;
using TASHPAV11.Mapping;
using TASHPAV11.Model;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace TASHPAV11.Pages.Courses
{
    public class ManageCoursesModel : PageModel
    {

        public string active_input { get; set; }
        public string submitNewButton { get; set; }
        public string delete_input { get; set; }
        public string deleteButton { get; set; }
        public string msg { get; set; }
        public string DisplayList { get; set; }
        public string insertMSG { get; set; }
        public string insert_button { get; set; }
        [BindProperty]
        public string deleteMSG { get; set; }
        public CourseWithPersonList List { get; set; } = new CourseWithPersonList();
        public Course course { get; set; } = new Course();
        [BindProperty]
        public Course dCourse { get; set; }
        [BindProperty]
        public CourseWithPerson newCourse { get; set; }

        private readonly string _apiKey;

        public ManageCoursesModel(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new Exception("OpenAI API key not found");
        }



        public void OnGet()
        {
            active_input = "display:none";
            submitNewButton = "display:none";
            DisplayList = "display:block";
            delete_input = "display:none";
            deleteButton = "display:none";
            CoursesDB db = new CoursesDB();
            List = db.SelectAll();
        }
        public void OnPostRenderCourses()
        {
            active_input = "display:none";
            submitNewButton = "display:none";
            DisplayList = "display:block";
            delete_input = "display:none";
            deleteButton = "display:none";
            CoursesDB db = new CoursesDB();
            List = db.SelectAll();
        }

        public void OnPostShowAddCourses()
        {
            DisplayList = "display:none";
            active_input = "display:block";
            insert_button = "display:block";
            delete_input = "display:none";
            deleteButton = "display:none";
            
        }
        public void OnPostShowDeleteCourse()
        {
            DisplayList = "display:none";
            active_input = "display:none";
            insert_button = "display:none";
            delete_input = "display:block";
            deleteButton = "display:block";
            CoursesDB db = new CoursesDB();
            List = db.SelectAll();
        }
        public void OnPostInsertCourse(string Department)
        {
            if (Department == "MathReq") { newCourse.Course.MathReq = true; newCourse.Course.ComuterReq = false; newCourse.Course.AdvancedSelection = false; }
            if (Department == "ComputerReq") { newCourse.Course.MathReq = false; newCourse.Course.ComuterReq = true; newCourse.Course.AdvancedSelection = false; }
            if (Department == "Advance") { newCourse.Course.MathReq = false; newCourse.Course.ComuterReq = false; newCourse.Course.AdvancedSelection = true; }
            CoursesDB db = new CoursesDB();
            int records = db.Insert(newCourse);
            delete_input = "display:none";
            deleteButton = "display:none";
            if (records == -1)
            {
                insertMSG = "Course with same name or number already exist";
            }
            else if (records == 1)
            {
                insert_button = "disply:none";
                insertMSG = "Course Added Successfuly";
            }
            else { insertMSG = "Could Not Add Course !!!"; }
            ;
        }
        public void OnPostDeleteCourse(int CourseToDelete)
        {

            DisplayList = "display:none";
            active_input = "display:none";
            insert_button = "display:none";
            delete_input = "display:block";
            deleteButton = "display:block";

            CoursesDB db = new CoursesDB();

            int records = db.DeleteCourse(CourseToDelete);
            if (records == 1) { deleteMSG = "Course Deleted Successfuly"; }
            else { deleteMSG = "Could Not Delete Course !!!"; }
            ;
        }

        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<string> AskChatGPTAsync(string question)
        {
            //string? apiKey =
            //    Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("OpenAI API key is missing.");

            var requestData = new
            {
                model = "gpt-5-mini",
                input = question
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.openai.com/v1/responses"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            request.Content = JsonContent.Create(requestData);

            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(json);

            foreach (var item in doc.RootElement.GetProperty("output").EnumerateArray())
            {
                if (item.GetProperty("type").GetString() != "message")
                    continue;

                foreach (var content in item.GetProperty("content").EnumerateArray())
                {
                    if (content.GetProperty("type").GetString() == "output_text")
                        return content.GetProperty("text").GetString() ?? "";
                }
            }

            return "No answer was returned.";
        }

        public async Task<IActionResult> OnPostExplainAsync()
        {
            string answer = await AskChatGPTAsync(
                "Explain Newton's second law to a high school student."
            );

            return Content(answer);
        }


    }
}

