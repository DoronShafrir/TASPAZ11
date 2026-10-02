namespace TASHPAV11.Model
{
    public class Course 
    {
        public int CId { get; set; }
        public string CourseName { get; set; }
        public int CourseNumber { get; set; }
        public int Prerequisites_1 { get; set; }
        public int Prerequisites_2 { get; set; }
        public int Prerequisites_3 { get; set; }
        public int Credits { get; set; }
        public bool MathReq { get; set; }
        public bool ComuterReq { get; set; }
        public bool AdvancedSelection { get; set; }

    }

    public class Coursess : List<Course>
    {
        public Coursess() { }

        public Coursess(IEnumerable<Course> list)
            : base(list)
        {

        }
    }
}
