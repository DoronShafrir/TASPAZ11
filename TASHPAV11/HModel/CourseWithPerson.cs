using TASHPAV11.HModel;
using TASHPAV11.Model;

namespace TASHPAV11.HModel
{
    public  class CourseWithPerson
    {
        public Person person { get; set; } = new Person();
        public Course course { get; set; } = new Course();
        public Student student { get; set; } = new Student();
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
