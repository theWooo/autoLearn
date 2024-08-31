using diplom.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net.Http.Headers;

namespace diplom.Controllers {
    public class CourseController : Controller {
        [Authorize]
        public async Task<IActionResult> Index() {
            string email = HttpContext.User.Claims.First(it => it.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            CourseBatchDTO batch = new CourseBatchDTO();
            SqlDataReader reader = await DI.getDiContainer().asyncExecuteReader(@$"select coursename, courseDescription,courseImageLink from course join coursetooperator on courseidfk = course.id join operator on operatoridfk = operator.id join auth on auth.id = operator.authfk where email = '{email}'");
            while (reader.Read()) {
                batch.courses.Add(new Course() { courseDescription = reader.GetValue(1) as string, courseName = reader.GetValue(0) as string,courseImageLink = reader.GetValue(2) as string});
            }
            return View(batch);
        }
        [Authorize]
        public async Task<IActionResult> CourseWorkshop() {
            CourseBatchDTO batch = new CourseBatchDTO();
            SqlDataReader reader = await DI.getDiContainer().asyncExecuteReader(@$"select courseName,courseDescription,courseImageLink,id,courseImage from course where creatoridfk = {HttpContext.User.Claims.First(it => it.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication").Value}");
            while (reader.Read()) {
                batch.courses.Add(new Course { courseDescription=reader.GetValue(1) as string, courseImageLink=(reader.GetValue(2)) as string, courseName=reader.GetValue(0) as string,courseId = (int)reader.GetValue(3),courseImageDataString = reader.GetValue(4) as string });
            }
            return View(batch);
        }
        [Authorize]
        public IActionResult RedactSelectedCourse() {
            return View();
        }
        [Authorize]
        public IActionResult CreateCourse() {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseDataDTO data) {
            if (!ModelState.IsValid) { return View(); }
            
            await DI.getDiContainer().asyncExecuteNonQuery($"insert into course(coursename,courseImage,courseDescription,creatorIdFK) values ('{data.courseHeaderData.courseName}','{$"data:image/gif;base64,{DI.getDiContainer().getImageData(data.courseHeaderData.courseImageData)}"}','{data.courseHeaderData.courseDescription}',{HttpContext.User.Claims.First(it=>it.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication").Value})");
            return RedirectToAction("CourseWorkshop", "Course");
        }
        [Authorize]
        [HttpPost]
        public IActionResult SubmitCourseChanges() {
            return RedirectToAction("CourseWorkshop", "Course");
        }
    }
}