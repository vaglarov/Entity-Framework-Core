using System;
using System.Data.SqlClient;

namespace _009.IncreaseAgeStoredProcedure
{
    class IncreaseAgeStoredProcedure
    {
        private const string DB_NAME = "MinionsDB";

        private static string connectionString =
                "Server=localhost\\SQLEXPRESS;" +
                $"Database={DB_NAME};" +
                "Integrated Security= true;";

        private static SqlConnection connection = new SqlConnection(string.Format(connectionString, DB_NAME));
        public static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            using (connection)
            {
                connection.Open();

                string createProc = @"CREATE PROC usp_GetOlder @id INT
                                      AS
                                      UPDATE Minions
                                        SET Age += 1
                                      WHERE Id = @id";

                using (SqlCommand command = new SqlCommand(createProc, connection))
                {
                    command.ExecuteNonQuery();
                }

                string execProc = @"EXEC usp_GetOlder @id";

                using (SqlCommand command = new SqlCommand(execProc, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }

                string selectQuery = @"SELECT Name, Age FROM Minions WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{(string)reader[0]} – {(int)reader[1]} years old");
                        }
                    }
                }
            }
        }
    }
}
