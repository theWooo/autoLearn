using System.ComponentModel.DataAnnotations;

namespace diplom.Models {
    public class AuthorizationData {
        [Required]
        public string login;
        [Required]
        public string password;
    }
}
