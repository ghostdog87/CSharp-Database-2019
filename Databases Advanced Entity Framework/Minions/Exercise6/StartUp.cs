using Minions;
using System;
using System.Data.SqlClient;

namespace Exercise6
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.connectionString))
            {
                connection.Open();

                string villainName;

                string selectVillianQuery = "SELECT Name FROM Villains WHERE Id = @villainId";

                using(SqlCommand command = new SqlCommand(selectVillianQuery, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villainId);

                    villainName = (string)command.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine("No such villain was found.");
                        return;
                    }                    
                }


                int rowsAffected = DeleteMinionsInVillain(connection, villainId);
                DeleteVillain(connection, villainId);

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{rowsAffected} minions were released.");
            }
        }

        private static void DeleteVillain(SqlConnection connection, int villainId)
        {
            string deleteVillain = "DELETE FROM Villains WHERE Id = @villainId";

            using (SqlCommand command = new SqlCommand(deleteVillain, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);

                command.ExecuteNonQuery();
            }
        }

        private static int DeleteMinionsInVillain(SqlConnection connection, int villainId)
        {
            string deleteMinionsInVillain = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";

            using (SqlCommand command = new SqlCommand(deleteMinionsInVillain, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);

                return command.ExecuteNonQuery();

            }
        }
    }
}
