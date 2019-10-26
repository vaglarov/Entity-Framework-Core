using System;
using System.Data.SqlClient;

namespace _2.VillainNames
{
    class VillainNames
    {


        private const string DB_NAME = "MinionsDB";

        private static string connectionString =
                "Server=localhost\\SQLEXPRESS;" +
                $"Database={DB_NAME};" +
                "Integrated Security= true;";

        private static SqlConnection connection = new SqlConnection(string.Format(connectionString, DB_NAME));
        static void Main(string[] args)
        {

            connection.Open();
            using (connection)
            {
                string queryText = @"  SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                            FROM Villains AS v 
                                            JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                        GROUP BY v.Id, v.Name 
                                          HAVING COUNT(mv.VillainId) > 3 
                                        ORDER BY COUNT(mv.VillainId)";
                try
                {

                    SqlCommand cmd = new SqlCommand(queryText, connection);

                    SqlDataReader reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            var name = reader["Name"].ToString();
                            var minionsCount = reader["MinionsCount"].ToString();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($"{name}");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($" - ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($"{minionsCount}");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.White;

                        }
                    }
                }
                catch (Exception e )
                {

                    Console.WriteLine(e.Message);
                }


            }
        }
    }
}
