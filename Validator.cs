using diplom.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace diplom
{
    public class Validator
    {
        //private static Regex emailRegex = new Regex(@"^([a-z]|[A-Z]|[0-9])+\@[a-z]+\.[a-z]+$");
        private bool isPasswordValid(string password, bool isUppercaseRequired=true, bool isSymbolRequired = true, bool isNumberRequired = true, int minPasswordLength = 8) {
            if (string.IsNullOrEmpty(password)) { return false; }
            if (isUppercaseRequired && !password.Any(it => char.IsUpper(it))) { return false; }
            if (isSymbolRequired && !password.Any(it => char.IsSymbol(it))) { return false; }
            if (isNumberRequired && !password.Any(it=>char.IsNumber(it))) { return false; }
            if (password.Length < minPasswordLength) { return false; }
            return true;
        }
        public async Task<bool> isRegistrationDataValid(RegistrationData data, ModelStateDictionary errors) {
            if (!errors.IsValid) {
                return false;
            }
            if (!isPasswordValid(data.password)) {
                errors.AddModelError("invPassword","Пароль некорректен");
                return false;
            }
            if (await isEmailTaken(data.email)) {
                errors.AddModelError("emailIsTaken", "Такой адрес почты уже используется");
                return false;
            }
            return true;
        }
        private async Task<bool> isEmailTaken(object email) {
            
            string castedEmail = (string)(email);
            int res = (int)await DI.getDiContainer().asyncExecuteScalar($"SELECT COUNT(EMAIL) FROM AUTH WHERE EMAIL = '{castedEmail}'");
            if ((int)res! > 0) {
                return true;
            }
            //TODO - ask db if this email is present there if it is then ret false otherwise ret true;
            return false;
        }

        //public async Task<bool> isEmailValid(string email) {
        //    bool adCallResult = await Task<bool>.Factory.StartNew(isEmailTaken, email as object); // mb problem here
        //    return emailRegex.IsMatch(email) && adCallResult;
        //}
    }
}