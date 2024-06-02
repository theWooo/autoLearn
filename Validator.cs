using diplom.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace diplom
{
    public class Validator
    {
        private static Regex emailRegex = new Regex(@"^([a-z]|[A-Z]|[0-9])+\@[a-z]+\.[a-z]+$");
        public bool isPassordValid(string password, bool isUppercaseRequired=true, bool isSymbolRequired = true, bool isNumberRequired = true, int minPasswordLength = 8) {
            if (string.IsNullOrEmpty(password)) { return false; }
            if (isUppercaseRequired && !password.Any(it => char.IsUpper(it))) { return false; }
            if (isSymbolRequired && !password.Any(it => char.IsSymbol(it))) { return false; }
            if (isNumberRequired && !password.Any(it=>char.IsNumber(it))) { return false; }
            if (password.Length < minPasswordLength) { return false; }
            return true;
        }
        public async Task<bool> isEmailTaken(object email) {
            string castedEmail = (string)(email);
            SqlCommand command = new SqlCommand(
                $"SELECT COUNT(EMAIL) FROM STUDENT WHERE EMAIL = '{castedEmail}'"
                , DI.getDiContainer().dbConnection);
            object? res = await command.ExecuteScalarAsync();
            
            if ((int)res! > 0) {
                return false;
            }
            //TODO - ask db if this email is present there if it is then ret false otherwise ret true;
            return true;
        }

        //public async Task<bool> isEmailValid(string email) {
        //    bool adCallResult = await Task<bool>.Factory.StartNew(isEmailTaken, email as object); // mb problem here
        //    return emailRegex.IsMatch(email) && adCallResult;
        //}
    }
}