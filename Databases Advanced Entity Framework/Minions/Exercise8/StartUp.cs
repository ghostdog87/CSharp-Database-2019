using Minions;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Exercise8
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            int[] ids = Console.ReadLine().Split().Select(int.Parse).ToArray();

            foreach (var id in ids)
            {
                using (SqlConnection connection = new SqlConnection(Configuration.connectionString))
                {
                    connection.Open();

                    string updateMinion = "UPDATE Minions SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1 WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(updateMinion, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(Configuration.connectionString))
            {
                connection.Open();

                string selectMinions = "SELECT Name, Age FROM Minions";

                using (SqlCommand command = new SqlCommand(selectMinions, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string minionName = (string)reader[0];
                            int minionAge = (int)reader[1];

                            Console.WriteLine($"{minionName} {minionAge}");
                        }
                    }
                    
                }
            }

        }
    }
}
