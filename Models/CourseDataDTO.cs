using System.ComponentModel.DataAnnotations;

namespace diplom.Models {
    public class CourseDataDTO {
        [Required]
        public Course courseHeaderData { get; set; }
    }
}
