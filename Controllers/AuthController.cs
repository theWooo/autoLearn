using diplom.Models;
using Microsoft.AspNetCore.Mvc;

namespace diplom.Controllers {
    public class AuthController : Controller {
        private DI container = DI.getDiContainer();
        public IActionResult Index() {
            return View();
        }
        public IActionResult Authorization() {
            return View();
        }
        public IActionResult Register() {

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Register([Bind(include: "login,password,email,secretQuestion,answerToTheSecretQuestion")] RegistrationData data) {

            //if (DI.dbConnection) { 

            //}

            if (!isPasswordValid()) {
                ModelState.AddModelError("invPassword", "Некорректный пароль");
            }
            return View();
        }
        //[HttpPost]
        //public IActionResult Authorization(AuthorizationData data) { 

        //}
    }
}