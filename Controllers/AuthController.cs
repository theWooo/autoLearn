using diplom.Models;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Unicode;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public IActionResult DeAuthorization() {
            HttpContext.Response.Cookies.Delete("token");
            return RedirectToAction("Index","Home");
        }
        public IActionResult Register() {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationData data) {
            if (!await DI.getDiContainer().validator.isRegistrationDataValid(data, ModelState)) {
                return View();
            }
            else {
                int res = Convert.ToInt32(((decimal)await DI.getDiContainer().asyncExecuteScalar($"INSERT INTO AUTH (PASSWORDHASH,EMAIL) VALUES ('{DI.getDiContainer().quickHash(data.password)}','{data.email}');select scope_identity()")));
                await DI.getDiContainer().asyncExecuteNonQuery($"INSERT INTO secretQuestion (answerHash,question,authFK) VALUES ('{DI.getDiContainer().quickHash(data.answerToTheSecretQuestion)}','{data.secretQuestion}',{res})");
                await DI.getDiContainer().asyncExecuteNonQuery($"INSERT INTO operator (operatorname,authfk) VALUES ('{data.login}',{res})");
            }
            //Response.Cookies.Append("token", generateToken());
            HttpContext.Response.Cookies.Append("token", "true");
            return RedirectToAction("Index","Home");
        }

        private string generateToken() {
            throw new NotImplementedException();
        }
        //[HttpPost]
        //public IActionResult Authorization(AuthorizationData data) { 

        //}
    }
}