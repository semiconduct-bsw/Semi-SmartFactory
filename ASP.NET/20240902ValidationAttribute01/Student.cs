using System.ComponentModel.DataAnnotations;

namespace _20240902_ValidationAttribute.Models
{
    public class Student
    {
        [Required(ErrorMessage = "이름을 작성해 주세요")]
        [StringLength(15, MinimumLength = 2, 
            ErrorMessage ="이름은 최소 2자 이상 15자 이하로 작성해주세요")]
        public string Name { get; set; }

        // ? = Null 일 수 있다는 의미
        [Required(ErrorMessage = "나이를 작성해 주세요")]
        [Range(20, 75,
            ErrorMessage = "20세~75세로 작성해주세요")]
        public int? Age { get; set; }
        public string Password { get; set; }
    }
}
