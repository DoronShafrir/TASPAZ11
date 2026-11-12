using TASHPAV11.HModel;
using TASHPAV11.Model;

namespace TASHPAV11.HModel
{
    public  class CourseWithPerson
    {
        public  Course course { get; set; }
        public  Person person { get; set; } 
        public Student student { get; set; }
    }
}
public  class CourseWithPersonList : List<CourseWithPerson>
{
    public CourseWithPersonList() { }
    public CourseWithPersonList(IEnumerable<CourseWithPerson> list)
        : base(list)
    {
    }
}
