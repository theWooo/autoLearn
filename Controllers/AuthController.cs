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

            //if (DI.dbConnection) { 

            //}

           if (!container.validator.isPassordValid(data.password)) {
               ModelState.AddModelError("invPassword", "Некорректный пароль");
           }
           if (await container.validator.isEmailTaken(data.email)) { 
           
           }
            return View();
        }
        //[HttpPost]
        //public IActionResult Authorization(AuthorizationData data) { 

        //}
    }
}