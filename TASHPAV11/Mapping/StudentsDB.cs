using System.Data.OleDb;
using System.Security.Cryptography;
using System.Transactions;
using System.Xml.Linq;
using TASHPAV11.App_Code;
using TASHPAV11.HModel;
using TASHPAV11.Model;

namespace TASHPAV11.Mapping
{
    public class StudentsDB
    {
        private readonly string connectionString = Imp_Data.ConString;


        public CourseWithPersonList StudentSelectAll(int studentId, bool except)
        {
            CourseWithPersonList courses = new CourseWithPersonList();
            string sql = string.Empty;
            if (except)
            {
                sql = $"SELECT  Courses.CId, Courses.CourseName, Courses.CourseNumber, Person.Name," +
                               " Courses.Prerequisites_1, Courses.Prerequisites_2, Courses.Prerequisites_3, Courses.Credits, Courses.MathReq, Courses.ComuterReq, Courses.AdvancedSelection" +
                               " FROM  ( ( [Student] INNER JOIN [Courses] ON [Courses].[CId] = [Student].[CourseId] )" +
                               "INNER JOIN [CourseTeacher] ON [Courses].[CId] = [CourseTeacher].[CourseId] )" +
                               " INNER JOIN [Person] ON [CourseTeacher].[TeacherId] = [Person].[Id] " +
                               $" WHERE {studentId} = [Student].[SId];";
            }
            else
            {
                sql = $"SELECT Courses.CId, Courses.CourseName, Courses.CourseNumber, Person.Name," +
                               " Courses.Prerequisites_1, Courses.Prerequisites_2, Courses.Prerequisites_3, Courses.Credits, Courses.MathReq, Courses.ComuterReq, Courses.AdvancedSelection" +
                               " FROM ( Courses INNER JOIN CourseTeacher ON Courses.CId = CourseTeacher.CourseId )" +
                               " INNER JOIN Person ON CourseTeacher.TeacherId = Person.Id " +
                              $"WHERE  Courses.CId NOT IN (SELECT Student.CourseId FROM Student WHERE Student.SId = {studentId}) ;";
            }

            using var connection = new OleDbConnection(connectionString);
            using var command = new OleDbCommand(sql, connection);

            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader!.Read())
            {
                CourseWithPerson course = new CourseWithPerson();
                {
                    course.Course = new Course
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
                    course.Person = new Person
                    {
                        Name = reader["Name"].ToString()
                    };
                }
                courses.Add(course);
            }

            return courses;
        }

        public int Insert(int SId, int CourseId)
        {

            int records = 0;

            string sql = $"INSERT INTO Student ([SId], [CourseId]) VALUES (?,?);";
            using var connection = new OleDbConnection(connectionString);
            using (OleDbCommand cmd = new OleDbCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("?", SId.ToString());
                cmd.Parameters.AddWithValue("?", CourseId.ToString());

                connection.Open();

                records = (int)cmd.ExecuteNonQuery();
            }

            return records;
        }

        public int DeleteCourse(Course course)
        {
            int records = 0;
            //string arg1 = course.CourseName;
            //string arg2 = course.CourseNumber;
            //int arg3 = course.ResponsibleTeacher;
            ////if (!(arg3 > 0)) return 0;

            //string sql = "DELETE FROM Courses  " +
            //    $"WHERE CourseName ='{arg1}' OR CourseNumber = '{arg2}' OR ResponsibleTeacher = {arg3}; ";

            //using var connection = new OleDbConnection(connectionString);
            //using var command = new OleDbCommand(sql, connection);

            //connection.Open();

            //records = command.ExecuteNonQuery();


            return records;
        }

        public int CheckName(string name)
        {
            int recordId = 0;
            string sql = $"SELECT Id FROM Person WHERE Name = '{name}';";
            using var connection = new OleDbConnection(connectionString);
            using var command = new OleDbCommand(sql, connection);
            connection.Open();
            recordId = (int)command.ExecuteScalar();

            return recordId;
        }
    }
}

