using System;
using System.Data.SqlClient;
using System.Linq;

namespace _004._AddMinion
{
    class AddMinion
    {

        private const string DB_NAME = "MinionsDB";

        private static string connectionString =
                "Server=localhost\\SQLEXPRESS;" +
                $"Database={DB_NAME};" +
                "Integrated Security= true;";

        private static SqlConnection connection = new SqlConnection(string.Format(connectionString, DB_NAME));
        static void Main(string[] args)
        {

            //Minion: Bob 14 Berlin
            Minion minionInput = InputLineReturnMinion();
            string[] secondLineInput = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string villianName = secondLineInput[1];

            int? townId;
            int? villainId;
            int? minionId;

            connection.Open();
            using (connection)
            {
                townId = GetTownId(minionInput.Town, connection);
                if (townId == null)
                {
                    AddTown(minionInput.Town, connection);
                }

                townId = GetTownId(minionInput.Town, connection);

                villainId = GetVillainId(villianName, connection);

                if (villainId == null)
                {
                    AddVillain(villianName, connection);
                }

                villainId = GetVillainId(villianName, connection);

                minionId = GetMinionId(minionInput.Name, connection);

                if (minionId == null)
                {
                    AddOneMinion(minionInput.Name, minionInput.Age, townId, connection);
                }

                minionId = GetMinionId(minionInput.Name, connection);

                InsertIntoMinionsVillains(villainId, villianName, minionId, minionInput.Name, connection);
            }
        }


        private static void InsertIntoMinionsVillains(int? villainId,string vilianName, int? minionId, string minionName, SqlConnection connection)
        {
            string query = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                command.Parameters.AddWithValue("@minionId", minionId);

                command.ExecuteNonQuery();

                Console.WriteLine($"Successfully added {minionName} to be minion of {vilianName}.");
            }
        }

        private static void AddTown(string minionTown, SqlConnection connection)
        {
            string insertTown = @"INSERT INTO Towns (Name) VALUES (@townName)";

            using (SqlCommand command = new SqlCommand(insertTown, connection))
            {
                command.Parameters.AddWithValue("@townName", minionTown);

                command.ExecuteNonQuery();

                Console.WriteLine($"Town {minionTown} was added to the database.");
            }
        }

        private static int? GetMinionId(string minionName, SqlConnection connection)
        {
            string selectMinionId = @"SELECT Id FROM Minions WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(selectMinionId, connection))
            {
                command.Parameters.AddWithValue("@Name", minionName);

                return (int?)command.ExecuteScalar();
            }
        }

        private static void AddVillain(string villainName, SqlConnection connection)
        {
            string insertVillain = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(insertVillain, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                command.ExecuteNonQuery();

                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
        }

        private static int? GetVillainId(string villainName, SqlConnection connection)
        {
            string selectVillainId = @"SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(selectVillainId, connection))
            {
                command.Parameters.AddWithValue("@Name", villainName);

                return (int?)command.ExecuteScalar();
            }
        }

        private static int? GetTownId(string minionTown, SqlConnection connection)
        {
            string selectTownId = @"SELECT Id FROM Towns WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(selectTownId, connection))
            {
                command.Parameters.AddWithValue("@Name", minionTown);

                return (int?)command.ExecuteScalar();
            }
        }

        
        private static void AddOneMinion(string minionName, int minionAge, int? townId, SqlConnection connection)
        {
            string insertMinion = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            using (SqlCommand command = new SqlCommand(insertMinion, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", minionAge);
                command.Parameters.AddWithValue("@townId", townId);

                command.ExecuteNonQuery();
            }
        }
        private static Minion InputLineReturnMinion()
        {
            string[] firstLineInput = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string minionName = firstLineInput[1];
            int minionAge = int.Parse(firstLineInput[2]);

            string minionTown = firstLineInput[3];
            Minion inputMinion = new Minion(minionName, minionAge, minionTown);
            return inputMinion;
        }
    }

    class Minion
    {
        public Minion(string name, int age, string town)
        {
            this.Name = name;
            this.Age = age;
            this.Town = town;
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Town { get; set; }
    }
}
