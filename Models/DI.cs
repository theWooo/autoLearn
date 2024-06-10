using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace diplom.Models {
    
    public class DI {
        private static DI container;
        private string decryptKey;
        public string getToken() {
            return decryptKey;
        }
        public async Task<string> generateToken(AuthorizationData data) {
            SqlDataReader reader = await asyncExecuteReader($"select operatorname from operator join auth on authfk=auth.id where email='{data.email}'");
            await reader.ReadAsync();
            string userName = reader.GetValue(0) as string;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] byteKey = Encoding.ASCII.GetBytes(DI.getDiContainer().getToken());
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Email, data.email)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(1000),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(byteKey), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
        public List<Claim> getTokenValues(string token) => new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.ToList();
        public string quickHash(string input) {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash);
        }
        /// <summary>
        /// Just executes sql-query and returns the amount of affected rows of data. Suits expressions like INSERT, UPDATE, DELETE.
        /// </summary>
        /// <param name="query">sql query</param>
        /// <returns></returns>
        public async Task<int> asyncExecuteNonQuery(string query) {
            SqlCommand command = new SqlCommand(query, DI.getDiContainer().dbConnection);
            return await command.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Executes sql query and returns a single value (integer for example). Suits sql expressions like SELECT combined with one of the inbuilt SQL functions like Min, Max, Sum or Count.
        /// </summary>
        /// <param name="query">sql query</param>
        /// <returns></returns>
        public async Task<object?> asyncExecuteScalar(string query) {
            SqlCommand command = new SqlCommand(query, DI.getDiContainer().dbConnection);
            return await command.ExecuteScalarAsync();
        }
        /// <summary>
        /// Executes sql-expression and returns rows from target table. Suits SELECT queries.
        /// </summary>
        /// <param name="query">sql query</param>
        /// <returns></returns>
        public async Task<SqlDataReader> asyncExecuteReader(string query) {
            SqlCommand command = new SqlCommand(query, DI.getDiContainer().dbConnection);
            return await command.ExecuteReaderAsync();
        }
        public static DI getDiContainer(string pathToFileContainingConnectionString = "") {
            if (container == null) {
                container = new DI(pathToFileContainingConnectionString);
                container.decryptKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["tokenDecryptionKey"];
                return container;
            }
            else {
                return container;
            }
        }
        public SqlConnection dbConnection;
        public Validator validator;
        private DI(string pathToFileContainingConnectionString)
        {
            validator = new Validator();
            if (!File.Exists(pathToFileContainingConnectionString)) {
                throw new FileNotFoundException();
            }
            StreamReader reader = new StreamReader(File.OpenRead(pathToFileContainingConnectionString));
            dbConnection = new SqlConnection(reader.ReadToEnd());
            reader.Close();
            dbConnection.Open();
        }

    }
}