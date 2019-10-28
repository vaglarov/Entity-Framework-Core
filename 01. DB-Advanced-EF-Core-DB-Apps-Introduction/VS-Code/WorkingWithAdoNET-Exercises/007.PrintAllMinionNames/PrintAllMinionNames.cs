using System;
using System.Data.SqlClient;

namespace _007.PrintAllMinionNames
{
    class PrintAllMinionNames
    {
        private const string DB_NAME = "MinionsDB";

        private static string connectionString =
                "Server=localhost\\SQLEXPRESS;" +
                $"Database={DB_NAME};" +
                "Integrated Security= true;";

        private static SqlConnection connection = new SqlConnection(string.Format(connectionString, DB_NAME));

        public static int firstId;

        public static int lastId;
        public static void Main(string[] args)
        {
            using (connection)
            {
                connection.Open();

                InitializeFirstId(connection);
                InitializeLastId(connection);

                int halfDiff = (lastId) / 2;
                bool isOdd = false;

                if (lastId % 2 != 0)
                {
                    isOdd = true;
                }

                while (lastId >= (halfDiff + 1) && firstId <= (halfDiff))
                {
                    Console.WriteLine(GetMinionName(firstId, connection));
                    Console.WriteLine(GetMinionName(lastId, connection));

                    firstId++;
                    lastId--;
                }

                if (isOdd)
                {
                    Console.WriteLine(GetMinionName(halfDiff + 1, connection));
                }
            }
        }

        private static void InitializeLastId(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT MAX(Id) FROM Minions", connection))
            {
                lastId = (int)cmd.ExecuteScalar();
            }
        }

        private static void InitializeFirstId(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT MIN(Id) FROM Minions", connection))
            {
                firstId = (int)cmd.ExecuteScalar();
            }
        }

        private static string GetMinionName(int id, SqlConnection connection)
        {
            string selectMinionName = @"SELECT Name FROM Minions WHERE Id = @minionId";

            using (SqlCommand cmd = new SqlCommand(selectMinionName, connection))
            {
                cmd.Parameters.AddWithValue("@minionId", id);
                return (string)cmd.ExecuteScalar();
            }
        }
    }
}
