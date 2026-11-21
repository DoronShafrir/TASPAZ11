using System.Data.OleDb;
using TASHPAV11.App_Code;
using TASHPAV11.HModel;
using TASHPAV11.Model;

namespace TASHPAV11.Mapping
{
    public class MarksDB
    {
        private readonly string connectionString = Imp_Data.ConString;
        public CourseWithPersonList SelectAllByTeacher(int teacherId, int courseId)
        {
            CourseWithPersonList courses = new CourseWithPersonList();
            

            const string sql = @"SELECT  Courses.CId,
            Courses.CourseName,
            Courses.CourseNumber,
            Person.Name,
            Courses.Prerequisites_1,
            Courses.Prerequisites_2,
            Courses.Prerequisites_3,
            Courses.Credits,
            Courses.MathReq,
            Courses.ComuterReq,
            Courses.AdvancedSelection
        FROM
            (Courses
            LEFT JOIN CourseTeacher
                ON Courses.CId = CourseTeacher.CourseId)
            LEFT JOIN Person
                ON CourseTeacher.TeacherId = Person.Id
            WHERE CourseTeacher.TeacherId = ? AND  (? = 0 OR Courses.CId = ?);";

            using var connection = new OleDbConnection(connectionString);
            using var command = new OleDbCommand(sql, connection);
            command.Parameters.AddWithValue("?", teacherId);
            command.Parameters.AddWithValue("?", courseId);
            command.Parameters.AddWithValue("?", courseId);

            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader!.Read())
            {
                CourseWithPerson course = new CourseWithPerson();
                {
                    course.course = new Course
                    {
                        CId = int.Parse(reader["CId"].ToString()),
                        CourseName = reader["CourseName"].ToString(),
                        CourseNumber = int.Parse(reader["CourseNumber"].ToString()),
                        Prerequisites_1 = int.Parse(reader["Prerequisites_1"].ToString()),
                        Prerequisites_2 = int.Parse(reader["Prerequisites_2"].ToString()),
                        Prerequisites_3 = int.Parse(reader["Prerequisites_3"].ToString()),
                        Credits = reader["Credits"] != DBNull.Value ? int.Parse(reader["Credits"].ToString()) : 0,
                        MathReq = reader["MathReq"] != DBNull.Value ? bool.Parse(reader["MathReq"].ToString()) : false,
                        ComuterReq = reader["ComuterReq"] != DBNull.Value ? bool.Parse(reader["ComuterReq"].ToString()) : false,
                        AdvancedSelection = reader["AdvancedSelection"] != DBNull.Value ? bool.Parse(reader["AdvancedSelection"].ToString()) : false
                    };
                    course.person = new Person
                    {
                        Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : ""
                    };
                }


                courses.Add(course);
            }
            return courses;
        }

        public CourseWithPersonList SelectStudentsByCourse(int teacherId, int CourseToShow)
        {
            CourseWithPersonList courses = new CourseWithPersonList();


            const string sql = @"SELECT  
            Person.Id,
            Person.Name,
            Courses.Credits,
            Student.Marks,
            Courses.CourseName
            
        FROM
           ( (Courses
            INNER JOIN CourseTeacher
                ON Courses.CId = CourseTeacher.CourseId)
            INNER JOIN Student
                ON Courses.CId = Student.CourseId)
            INNER JOIN Person
                ON   Student.SId=Person.Id

            WHERE CourseTeacher.TeacherId = ? AND Courses.CId = ?;";

            using var connection = new OleDbConnection(connectionString);
            using var command = new OleDbCommand(sql, connection);
            command.Parameters.AddWithValue("?", teacherId);
            command.Parameters.AddWithValue("?", CourseToShow);

            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader!.Read())
            {
                CourseWithPerson course = new CourseWithPerson();
                {
                    course.course = new Course
                    {
                        
                        CourseName = reader["CourseName"].ToString(),               
                        Credits = reader["Credits"] != DBNull.Value ? int.Parse(reader["Credits"].ToString()) : 0,
                        
                    };
                    course.person = new Person
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                      
                        Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : ""
                    };
                    course.student = new Student
                    {
                        Marks = reader["Marks"] != DBNull.Value ? int.Parse(reader["Marks"].ToString()): 0
                    };
                }

                courses.Add(course);
            }
            return courses;
        }
    }
}
