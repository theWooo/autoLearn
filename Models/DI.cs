using System.Data.SqlClient;

namespace diplom.Models {
    
    public class DI {
        public static SqlConnection dbConnection;
        public DI(string pathToFileContainingConnectionString)
        {
            if (!File.Exists(pathToFileContainingConnectionString)) {
                throw new FileNotFoundException();
            }
            StreamReader reader = new StreamReader(File.OpenRead(pathToFileContainingConnectionString));
            dbConnection = new SqlConnection(reader.ReadToEnd());
            reader.Close();
        }

    }
}
