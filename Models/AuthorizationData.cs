using System.ComponentModel.DataAnnotations;

namespace diplom.Models {
    public class AuthorizationData {
        [Required(ErrorMessage = "Поле почты не может быть пустым")]
        [EmailAddress(ErrorMessage = "Недействительный адрес почты")]
        public string email { get; set; }
        [MaxLength(25,ErrorMessage = "Пароль не может быть больше 25 символов")]
        [Required(ErrorMessage = "Поле пароля не может быть пустым")]
        [MinLength(8, ErrorMessage = "Пароль должен быть больше 8 символов")]
        public string password { get; set; }
    }
}