using Microsoft.AspNetCore.Mvc;

namespace diplom.Controllers {
    public class CourseController : Controller {
        public IActionResult Index() {
            byte[]? jwttoken;
            if (HttpContext.Session.TryGetValue("jwtToken", out jwttoken) == null) { 
            
            }
            return View();
        }
    }
}
