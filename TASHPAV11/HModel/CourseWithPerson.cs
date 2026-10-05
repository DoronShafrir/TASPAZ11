using TASHPAV11.HModel;
using TASHPAV11.Model;

namespace TASHPAV11.HModel
{
    public  class CourseWithPerson
    {
        public  Course Course { get; set; }
        public  Person Person { get; set; }  
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
