using System;
using System.Data.SqlClient;
using System.Linq;

namespace _008.IncreaseMinionAge
{
    class IncreaseMinionAge
    {
        private const string DB_NAME = "MinionsDB";

        private static string connectionString =
                "Server=localhost\\SQLEXPRESS;" +
                $"Database={DB_NAME};" +
                "Integrated Security= true;";

        private static SqlConnection connection = new SqlConnection(string.Format(connectionString, DB_NAME));
        public static void Main(string[] args)
        {
            int[] IDs = Console.ReadLine()
                        .Split()
                        .Select(int.Parse)
                        .ToArray();

            using (connection)
            {
                connection.Open();

                foreach (var id in IDs)
                {
                    UpdateMinionInfo(id, connection);
                }

                string cmdText = @"SELECT Name, Age FROM Minions";

                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} {reader[1]}");
                        }
                    }
                }
            }
        }

        private static void UpdateMinionInfo(int id, SqlConnection connection)
        {
            string cmdText = @"UPDATE Minions
                               SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                               WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
