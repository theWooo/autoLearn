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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            SqlDataReader reader = await DI.getDiContainer().asyncExecuteReader($"select operator.id,operatorname from operator join auth on authfk=auth.id where email='{data.email}'");
            await reader.ReadAsync();
            string id = ((int)reader.GetValue(0)).ToString();
            string userName = reader.GetValue(1) as string;
            if (!ModelState.IsValid) {
                return View();
            }
            SqlDataReader expectedUserData = await DI.getDiContainer().asyncExecuteReader($"SELECT * FROM Operator JOIN AUTH ON OPERATOR.authFK = AUTH.id WHERE passwordHash = '{DI.getDiContainer().quickHash(data.password)}' AND EMAIL = '{data.email}'");

            //-------------------------------------------DELETE (&& data.email != "asdf@sdf.aa") IN RELEASE ONLY FOR TESTING
            //-------------------------------------------DELETE (&& data.email != "asdf@sdf.aa") IN RELEASE ONLY FOR TESTING
            //-------------------------------------------DELETE (&& data.email != "asdf@sdf.aa") IN RELEASE ONLY FOR TESTING
            if ((!expectedUserData.HasRows || ModelState.ErrorCount != 0)&& data.email != "asdf@sdf.aa") {
                ModelState.AddModelError("invLoginCredentials", "Недействительные данные входа");
                return View();
            }
            //-------------------------------------------DELETE (&& data.email != "asdf@sdf.aa") IN RELEASE ONLY FOR TESTING
            //-------------------------------------------DELETE (&& data.email != "asdf@sdf.aa") IN RELEASE ONLY FOR TESTING
            //-------------------------------------------DELETE (&& data.email != "asdf@sdf.aa") IN RELEASE ONLY FOR TESTING
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, data.email),
                new Claim(ClaimTypes.Authentication, id)
            };
            ClaimsIdentity ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(ci);
            await HttpContext.SignInAsync(claimsPrincipal);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> DeAuthorization() {
            await HttpContext.SignOutAsync();
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
        
        
        //[HttpPost]
        //public IActionResult Authorization(AuthorizationData data) { 

        //}
    }
}