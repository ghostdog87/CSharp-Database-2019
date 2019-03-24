using Minions;
using System;
using System.Data.SqlClient;

namespace Exercise3
{
    public class Program
    {
        public static void Main(string[] args)
        {

            int villainID = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.connectionString))
            {
                connection.Open();

                string villains = @"SELECT Name FROM Villains WHERE Id = @id";

                using (SqlCommand command = new SqlCommand(villains, connection))
                {
                    command.Parameters.AddWithValue("@id", villainID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = (string)reader["Name"];

                            if (name == null)
                            {
                                Console.WriteLine($"No villain with ID {villainID} exists in the database.");
                            }
                            else
                            {
                                Console.WriteLine($"Villain: {name}");
                            }                            
                        }
                    }
                }

                string minions = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                       WHERE mv.VillainId = @id
                                    ORDER BY m.Name";

                using (SqlCommand command = new SqlCommand(minions, connection))
                {
                    command.Parameters.AddWithValue("@id", villainID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int counter = 1;

                        while (reader.Read())
                        {
                            string name = (string)reader["Name"];
                            int age = (int)reader["Age"];

                            if (name == null)
                            {
                                Console.WriteLine($"(no minions)");
                            }
                            else
                            {
                                Console.WriteLine($"{counter}. {name} {age}");
                            }
                            counter++;
                        }
                    }
                }
            }
        }
    }
}
