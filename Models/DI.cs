using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace diplom.Models {
    
    public class DI {
        private static DI container;
        public string quickHash(string input) {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash);
        }
        public async Task<int> asyncExecuteNonQuery(string query) {
            SqlCommand command = new SqlCommand(query, DI.getDiContainer().dbConnection);
            return await command.ExecuteNonQueryAsync();
        }
        public async Task<object?> asyncExecuteScalar(string query) {
            SqlCommand command = new SqlCommand(query, DI.getDiContainer().dbConnection);
            return await command.ExecuteScalarAsync();
        }
        public static DI getDiContainer(string pathToFileContainingConnectionString = "") {
            if (container == null) {
                container = new DI(pathToFileContainingConnectionString);
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