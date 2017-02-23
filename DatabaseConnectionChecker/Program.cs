using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DatabaseConnectionChecker
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var tableValue = GetParameterFromArgs(args, 0, "language");
            var schemaValue = GetParameterFromArgs(args, 1, "cnfg");

            var messageResult = QueryAnything(tableValue, schemaValue);
            Console.WriteLine(messageResult);

            Console.ReadKey();
        }

        private static string GetParameterFromArgs(string[] args, int index, string defaultValue)
        {
            if (args.ElementAtOrDefault(index) != null)
            {
                return args.GetValue(index).ToString();
            }
            Console.WriteLine("The " + index + " variable is not defined");
            return defaultValue;
        }

        static string QueryAnything(string table, string schema)
        {
            try
            {
                var query = "SELECT * FROM [" + schema + "].[" + table + "]";

                var connection = ConfigurationManager.ConnectionStrings["ConnectionTest"].ConnectionString;

                using (var sqlConn = new SqlConnection(connection))
                {
                    using (var cmd = new SqlCommand(query, sqlConn))
                    {
                        sqlConn.Open();
                        var dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                    }
                }

                return "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine("Check the Connection String Name");
                return e.Message;
            }
        }
    }
}
