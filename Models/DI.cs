using System.Data.SqlClient;

namespace diplom.Models {
    
    public class DI {
        private static DI container;
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
        }

    }
}
