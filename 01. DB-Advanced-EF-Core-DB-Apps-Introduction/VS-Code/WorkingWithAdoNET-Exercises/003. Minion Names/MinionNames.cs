using System;
using System.Data;
using System.Data.SqlClient;

namespace _003._Minion_Names
{
    class MinionNames
    {
        private const string DN_NAME = "MinionsDB";

        private static string connectionString =
              "Server=localhost\\SQLEXPRESS;" +
              $"Database={DN_NAME};" +
              "Integrated Security= true;";


        private static SqlConnection connection = new SqlConnection(connectionString);
        static void Main(string[] args)
        {
            connection.Open();
            using (connection)
            {
                int villainID = int.Parse(Console.ReadLine());
                string selectVillainQuery = @"SELECT Name FROM Villains WHERE Id = @Id";

                SqlCommand villainCommand = new SqlCommand(selectVillainQuery, connection);

                using (villainCommand)
                {
                    villainCommand.Parameters.AddWithValue("@Id", villainID);

                    string villainName = (string)villainCommand.ExecuteScalar();
                    if (villainName == null)
                    {
                        Console.WriteLine($"No villain with ID {villainID} exists in the database.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Villain: {villainName}");
                        Console.ForegroundColor = ConsoleColor.White;

                        string selectMinionQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY m.Name) as RowNum,
                                                         m.Name, 
                                                         m.Age
                                                         FROM MinionsVillains AS mv
                                                         JOIN Minions As m ON mv.MinionId = m.Id
                                                        WHERE mv.VillainId = @Id
                                                        ORDER BY m.Name";

                        SqlCommand minionCommand = new SqlCommand(selectMinionQuery, connection);
                        using (SqlCommand minionsCommand = new SqlCommand(selectMinionQuery, connection))
                        {
                            minionsCommand.Parameters.AddWithValue("@Id", villainID);

                            using (SqlDataReader reader = minionsCommand.ExecuteReader())
                            {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                if (!reader.HasRows)
                                {
                                    Console.WriteLine("(no minions)");
                                    return;
                                }

                                while (reader.Read())
                                {
                                    Int64 rowNumber = (Int64)reader[0];
                                    string name = (string)reader[1];
                                    int age = (int)reader[2];
                                    Console.WriteLine($"{rowNumber}. {name} {age}");
                                }
                                Console.ForegroundColor = ConsoleColor.White;

                            }
                        }


                    }

                }
            }

        }

    }
}

