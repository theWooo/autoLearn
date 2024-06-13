namespace diplom.Models {
    public class CourseBatchDTO {
        public List<Course> courses = new List<Course>();
    }
    public class Course {
        public int courseId { get; set; }
        public string courseName { get; set; }
        public string courseDescription { get; set; }
        public string courseImageLink { get; set; }
    }
}
