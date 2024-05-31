using Microsoft.AspNetCore.Mvc;

namespace diplom.Controllers {
    public class CourseController : Controller {
        public IActionResult Index() {
            byte[]? token;
            if (HttpContext.Session.TryGetValue("token", out token) == null) { 
            
            }
            return View();
        }
    }
}
