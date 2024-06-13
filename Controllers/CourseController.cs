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
            CourseBatchDTO batch = new CourseBatchDTO();
            SqlDataReader reader = await DI.getDiContainer().asyncExecuteReader(@$"select coursename, courseDescription,courseImageLink from course join coursetooperator on courseidfk = course.id join operator on operatoridfk = operator.id join auth on auth.id = operator.authfk where email = '{email}'");
            while (reader.Read()) {
                batch.courses.Add(new Course() { courseDescription = reader.GetValue(1) as string, courseName = reader.GetValue(0) as string,courseImageLink = reader.GetValue(2) as string});
            }
            return View(batch);
        }
        public async Task<IActionResult> CourseWorkshop() {
            string token;
            HttpContext.Request.Cookies.TryGetValue("token", out token);
            if (token == null) {
                return RedirectToAction("Authorization", "Auth");
            }
            CourseBatchDTO batch = new CourseBatchDTO();
            SqlDataReader reader = await DI.getDiContainer().asyncExecuteReader(@$"select courseName,courseDescription,courseImageLink from course where creatoridfk = {DI.getDiContainer().getTokenValues(token).First(it => it.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication").Value}");//find all of all the courses that the current user has created by id
              //(TODO) above is a hack, this query has to be rewritten in a way in which it will utilize the designated creatoridfk field from the corresponding db table
            while (reader.Read()) {
                batch.courses.Add(new Course { courseDescription=reader.GetValue(1) as string, courseImageLink=reader.GetValue(2) as string, courseName=reader.GetValue(0) as string});
            }
            return View(batch);
        }
        public IActionResult SelectedCourse() {
            
            throw new NotImplementedException();
            return View();
        }
        public IActionResult CourseShop() {
            throw new NotImplementedException();
        }
        public IActionResult CreateCourse() {
            throw new NotImplementedException();
            return View();
        }
    }
}
