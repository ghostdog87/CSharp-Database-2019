using Minions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Exercise5
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(Configuration.connectionString))
            {

                connection.Open();

                string selectQuery = "UPDATE Towns " +
                                     "SET Name = UPPER(Name) " +
                                     "WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                using (SqlCommand updateTownNames = new SqlCommand(selectQuery, connection))
                {
                    updateTownNames.Parameters.AddWithValue("@countryName", countryName);

                    int rowsAffected = updateTownNames.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"{rowsAffected} town names were affected.");

                        string printQuery = "SELECT t.Name " +
                                            "FROM Towns as t " +
                                            "JOIN Countries AS c ON c.Id = t.CountryCode " +
                                            "WHERE c.Name = @countryName";

                        using (SqlCommand printTownNames = new SqlCommand(printQuery, connection))
                        {
                            printTownNames.Parameters.AddWithValue("@countryName", countryName);

                            var towns = printTownNames.ExecuteReader();

                            List<string> townNames = new List<string>();  

                            while (towns.Read())
                            {
                                townNames.Add((string)towns[0]);
                            }

                            Console.WriteLine("[" + string.Join(", ", townNames) + "]");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                }
            }
        }
    }
}
