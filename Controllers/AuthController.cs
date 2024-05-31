using diplom.Models;
using Microsoft.AspNetCore.Mvc;

namespace diplom.Controllers {
    public class AuthController : Controller {
        public IActionResult Index() {
            return View();
        }
        public IActionResult Authorization() {
            return View();
        }
        //[HttpPost]
        //public IActionResult Authorization(AuthorizationData data) { 
            
        //}
    }
}
