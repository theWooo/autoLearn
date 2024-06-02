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
        [HttpPost]
        public IActionResult Authorization(AuthorizationData data) {
            return View();
        }
        public IActionResult Register() {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationData data) {

            if (!ModelState.IsValid) {
                return View();
            }
            if (!container.validator.isPassordValid(data.password)) {
                ModelState.AddModelError("invPassword", "Некорректный пароль");
                return View();
            }
            if (await container.validator.isEmailTaken(data.email)) {
                ModelState.AddModelError("invEmail", "Email уже используется");
                return View();
            }
            return View();
        }
        //[HttpPost]
        //public IActionResult Authorization(AuthorizationData data) { 

        //}
    }
}