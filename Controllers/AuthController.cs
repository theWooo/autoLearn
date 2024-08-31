using diplom.Models;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Unicode;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace diplom.Controllers {
    public class AuthController : Controller {
        public IActionResult Index() {
            
            return View();
        }
        public IActionResult Authorization() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorization(AuthorizationData data) {
            if (!ModelState.IsValid) {
                return View();
            }
            SqlDataReader expectedUserData = await DI.getDiContainer().asyncExecuteReader($"SELECT * FROM Operator JOIN AUTH ON OPERATOR.authFK = AUTH.id WHERE passwordHash = '{DI.getDiContainer().quickHash(data.password)}' AND EMAIL = '{data.email}'");
            if (expectedUserData.HasRows && ModelState.ErrorCount == 0) {
                
            }
            else {
                ModelState.AddModelError("invLoginCredentials", "Недействительные данные входа");
                return View();
            }
            HttpContext.Response.Cookies.Append("token",await DI.getDiContainer().generateToken(data));
            return RedirectToAction("Index","Home");
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
            return RedirectToAction("Index","Home");
        }
    }
}