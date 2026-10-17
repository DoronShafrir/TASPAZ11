using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Transactions;
using TASHPAV11.App_Code;
using TASHPAV11.HModel;
using TASHPAV11.Model;
using CWP = TASHPAV11.HModel.CourseWithPerson;

namespace TASHPAV11.Mapping
{
    public class CoursesDB
    {
        private readonly string connectionString = Imp_Data.ConString;

        public CourseWithPersonList SelectAll()
        {
            CourseWithPersonList courses = new CourseWithPersonList();
            const string sql = "SELECT  Courses.CId, Courses.CourseName, Courses.CourseNumber, Person.Name," +
                                " Courses.Prerequisites_1, Courses.Prerequisites_2, Courses.Prerequisites_3, Courses.Credits, Courses.MathReq, Courses.ComuterReq, Courses.AdvancedSelection" +
                                " FROM  ( ( CourseTeacher INNER JOIN [Courses] ON [Courses].[CId] = [CourseTeacher].[CourseId] )" +
                                " INNER JOIN [Person] ON [CourseTeacher].[TeacherId] = [Person].[Id] );";

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


        public int Insert(CWP course)
        {
            int techerId = CheckName(course.Person.Name);
            if (techerId == 0) return 0;

            using var connection = new OleDbConnection(connectionString);
            connection.Open();

            //check if the course already exists in the database
            string sql = $"SELECT CId FROM Courses WHERE CourseName = ? OR CourseNumber = ?;";
           
            using (OleDbCommand cmd = new OleDbCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("?", course.Course.CourseName.Trim());
                cmd.Parameters.AddWithValue("?", course.Course.CourseNumber);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return -1; // course already exists
                }
                ; // course already exists

            }
             connection.Close();
             


            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                string sql1 = "Insert INTO Courses (CourseName, CourseNumber, Prerequisites_1, Prerequisites_2, Prerequisites_3," +
                    "Credits,  MathReq, ComuterReq, AdvancedSelection) VALUES ( ?, ?, ?, ?, ?, ?, ?,?,?) ; ";


                using (OleDbCommand cmd = new OleDbCommand(sql1, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("?", course.Course.CourseName);
                    cmd.Parameters.AddWithValue("?", course.Course.CourseNumber);
                    cmd.Parameters.AddWithValue("?", course.Course.Prerequisites_1);
                    cmd.Parameters.AddWithValue("?", course.Course.Prerequisites_2);
                    cmd.Parameters.AddWithValue("?", course.Course.Prerequisites_3);
                    cmd.Parameters.AddWithValue("?", course.Course.Credits);
                    cmd.Parameters.AddWithValue("?", course.Course.MathReq);
                    cmd.Parameters.AddWithValue("?", course.Course.ComuterReq);
                    cmd.Parameters.AddWithValue("?", course.Course.AdvancedSelection);

                    cmd.ExecuteNonQuery();
                }
                string sql2 = "Insert INTO CourseTeacher (CourseId, TeacherId) VALUES (?, ?);";
                // Get the CId generated by Access
                int courseId;


                //@@IDENTITY returns the last AutoNumber value generated on the current database connection
                using (OleDbCommand cmd = new OleDbCommand("SELECT @@IDENTITY", connection, transaction))
                {
                    courseId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (OleDbCommand cmd = new OleDbCommand(sql2, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("?", courseId);
                    cmd.Parameters.AddWithValue("?", techerId);

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return 1;
            }
            catch
            {
                transaction.Rollback();
                return 0;
            }
        }

        public int DeleteCourse(int courseId)
        {
            int records1, records2 = 0;

            using var connection = new OleDbConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                //Delete first from the table of teacher as a depndency and then from the courses table
                string sql1 = "DELETE FROM CourseTeacher  WHERE CourseId =? ;";
                using (OleDbCommand cmd = new OleDbCommand(sql1, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("?", courseId);
                    
                     records1 = (int)cmd.ExecuteNonQuery();
                }
                //Delete from the courses table
                string sql2 = "DELETE FROM Courses  WHERE CId =? ;";
                using (OleDbCommand cmd = new OleDbCommand(sql2, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("?", courseId);
                    
                     records2 = (int)cmd.ExecuteNonQuery();

                }
                if (records1 * records2 > 0)
                {
                    transaction.Commit();
                    return records2;
                }
                else
                {
                    transaction.Rollback();
                    return 0;
                }

            }
            catch
            {
                transaction.Rollback();
                return 0;
            }
           
        }

        public int CheckName(string name)
        {
            int recordId = 0;
            string sql = $"SELECT Id FROM Person WHERE Name = '{name}' AND Teacher = True ;";
            using var connection = new OleDbConnection(connectionString);
            using var command = new OleDbCommand(sql, connection);
            connection.Open();
            object? result = command.ExecuteScalar();

            return result == null ? 0 : Convert.ToInt32(result);
        }
    }
}
