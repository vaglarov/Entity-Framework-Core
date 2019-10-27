using System;
using System.Data.SqlClient;

namespace _006.RemoveVillain
{
    class RemoveVillain
    {

        private const string DB_NAME = "MinionsDB";

        private static string connectionString =
                "Server=localhost\\SQLEXPRESS;" +
                $"Database={DB_NAME};" +
                "Integrated Security= true;";

        private static SqlConnection connection = new SqlConnection(connectionString);

        private static SqlTransaction transaction;
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            connection.Open();
            using (connection)
            {
                transaction = connection.BeginTransaction();
                try
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;
                    cmd.CommandText = "SELECT Name FROM Villains WHERE Id = @villainId";
                    cmd.Parameters.AddWithValue("@villainId", id);
                    object value = cmd.ExecuteScalar();

                    if (value == null)
                    {
                        throw new NullReferenceException( "No such villain was found.");
                    }

                    string villainName = (string)value;

                    cmd.CommandText = @"DELETE FROM MinionsVillains 
                                        WHERE VillainId = @villainId";
                    int minionCountDeleted = cmd.ExecuteNonQuery();

                    cmd.CommandText = @"DELETE FROM Villains
                                         WHERE Id = @villainId";
                    cmd.ExecuteNonQuery();

                    transaction.Commit();

                    Console.WriteLine($"{villainName} was deleted.");
                    Console.WriteLine($"{minionCountDeleted} minions were released.");

                }
                catch (NullReferenceException e)
                {
                    try
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                    catch (Exception mes)
                    {
                        Console.WriteLine(mes.Message);
                    }

                }
                catch (Exception e)
                {
                    try
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                    catch (Exception rb)
                    {
                        Console.WriteLine(rb.Message);
                    }
                }
            }
        }
    }
}
