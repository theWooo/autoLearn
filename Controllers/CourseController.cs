using diplom.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
namespace diplom.Controllers {
    public class CourseController : Controller {
        [Authorize]
        public async Task<IActionResult> Index() {
            string email = HttpContext.User.Claims.First(it => it.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            CourseBatchDTO batch = new CourseBatchDTO();
            SqlDataReader reader = await DI.getDiContainer().asyncExecuteReader(@$"select coursename, courseDescription,courseImageLink, course.id,courseImage from course join coursetooperator on courseidfk = course.id join operator on operatoridfk = operator.id join auth on auth.id = operator.authfk");// where email = '{email}'
            while (reader.Read()) {
                batch.courses.Add(new Course() { courseDescription = reader.GetValue(1) as string, courseName = reader.GetValue(0) as string,courseImageLink = reader.GetValue(2) as string, courseId = int.Parse(reader.GetValue(3).ToString()),courseImageDataString = reader.GetValue(4) as string });
            }
            return View(batch);
        }
        [Authorize]
        public async Task<IActionResult> CourseWorkshop() {
            CourseBatchDTO batch = new CourseBatchDTO();
            SqlDataReader reader = await DI.getDiContainer().asyncExecuteReader(@$"select courseName,courseDescription,courseImageLink,id,courseImage from course where creatoridfk = {HttpContext.User.Claims.First(it => it.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication").Value}");
            while (reader.Read()) {
                batch.courses.Add(new Course { courseDescription=reader.GetValue(1) as string, courseImageLink=(reader.GetValue(2)) as string, courseName = reader.GetValue(0) as string,courseId = (int)reader.GetValue(3),courseImageDataString = reader.GetValue(4) as string });
            }
            return View(batch);
        }
        [Authorize]
        public async Task<IActionResult> DeleteCourse(int id) {
            DI.getDiContainer().asyncExecuteNonQuery($"delete from course where course.id = {id}");
            DI.getDiContainer().asyncExecuteNonQuery($"delete from chunk where courceFK = {id}");
            return RedirectToAction("CourseWorkshop", "Course");
        }
        [Authorize]
        public IActionResult RedactCourse() {
            return View();
        } 
        public IActionResult CreateCourse() {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> ViewSelectedCourse() {
            SqlDataReader courseDataReader = await DI.getDiContainer().asyncExecuteReader($"select course.courseName, course.courseDescription, chunk.chunkData from course join chunk on chunk.courceFK = course.id where course.id = '{int.Parse(Request.QueryString.ToString().Split("=").Last())}'");
            await courseDataReader.ReadAsync();
            var a = courseDataReader.GetValue(0);
            return View(new CourseViewTransferDTO() {courseName=courseDataReader.GetValue(0) as string, courseDescription = courseDataReader.GetValue(1) as string,courseContents = courseDataReader.GetValue(2) as string});
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseDataDTO data) {
            if (!ModelState.IsValid) { return View(); }

            int id  = Convert.ToInt32(await DI.getDiContainer().asyncExecuteScalar($"insert into course(coursename,courseImage,courseDescription,creatorIdFK,isEnabled) values ('{data.courseHeaderData.courseName}','{$"data:image/gif;base64,{DI.getDiContainer().getImageData(data.courseHeaderData.courseImageData)}"}','{data.courseHeaderData.courseDescription}',{HttpContext.User.Claims.First(it=>it.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication").Value},1);select scope_identity()"));
              await DI.getDiContainer().asyncExecuteNonQuery($"insert into chunk (chunkData,courceFK) values ('{Request.Form["content"]}',{id})");
              await DI.getDiContainer().asyncExecuteNonQuery($"insert into coursetooperator (courseIdFK,operatorIdFK) values ({id},{User.Claims.First(it=>it.Type== "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication").Value})");
            return RedirectToAction("CourseWorkshop", "Course");
        }
        [Authorize]
        [HttpPost]
        public IActionResult SubmitCourseChanges() {
            return RedirectToAction("CourseWorkshop", "Course");
        }
    }
}