using diplom.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace diplom.Controllers {
    public class CourseController : Controller {
        public async Task<IActionResult> Index() {
            string token;
            HttpContext.Request.Cookies.TryGetValue("token", out token);
            if (token == null) {
                return RedirectToAction("Authorization", "Auth");
            }
            string email = DI.getDiContainer().getTokenValues(token).First(it => it.Type == "email").Value;
            CourseDTO courseDTO = new CourseDTO();
            SqlDataReader reader = await DI.getDiContainer().asyncExecuteReader(@$"select coursename from course join coursetooperator on courseidfk = course.id join operator on operatoridfk = operator.id join auth on auth.id = operator.authfk where email = '{email}'");
            while (reader.Read()) {
                courseDTO.courseNames.Add(reader.GetValue(0) as string);
            }
            return View(courseDTO);
        }
        public IActionResult SelectedCourse() {
            return View();
        }
    }
}
