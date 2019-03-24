using Minions;
using System;
using System.Data.SqlClient;

namespace Exercise4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string[] minion = Console.ReadLine().Split();
            string[] villain = Console.ReadLine().Split();

            string minionName = minion[1];
            int minionAge = int.Parse(minion[2]);
            string minionTown = minion[3];

            string villainName = villain[1];

            using (SqlConnection connection = new SqlConnection(Configuration.connectionString))
            {
                string selectQuery = "SELECT Id FROM Towns WHERE Name = @townName";

                connection.Open();

                int? townID = null;
                int? villainID = null;
                int? minionID = null;

                using (SqlCommand selectTown = new SqlCommand(selectQuery, connection))
                {
                    selectTown.Parameters.AddWithValue("@townName", minionTown);

                    int? result = (int?)selectTown.ExecuteScalar();

                    if (result == null)
                    {
                        string insertQuery = "INSERT INTO Towns (Name) VALUES (@townName)";

                        using (SqlCommand insertTown = new SqlCommand(insertQuery, connection))
                        {
                            insertTown.Parameters.AddWithValue("@townName", minionTown);

                            int rows = insertTown.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                townID = (int?)selectTown.ExecuteScalar();
                                Console.WriteLine($"Town {minionTown} was added to the database.");
                            }
                        }
                    }
                    else
                    {
                        townID = result;
                    }
                }

                string selectVillainQuery = "SELECT Id FROM Villains WHERE Name = @Name";

                using (SqlCommand selectVillain = new SqlCommand(selectVillainQuery, connection))
                {
                    selectVillain.Parameters.AddWithValue("@Name", villainName);

                    int? result = (int?)selectVillain.ExecuteScalar();

                    if (result == null)
                    {
                        string insertQuery = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

                        using (SqlCommand insertVillain = new SqlCommand(insertQuery, connection))
                        {
                            insertVillain.Parameters.AddWithValue("@villainName", villainName);

                            int rows = insertVillain.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                villainID = (int?)selectVillain.ExecuteScalar();
                                Console.WriteLine($"Villain {villainName} was added to the database.");
                            }
                        }
                    }
                    else
                    {
                        villainID = result;
                    }
                }

                string insertMinionQuery = "SELECT Id FROM Minions WHERE Name = @Name";

                using (SqlCommand selectMinion = new SqlCommand(insertMinionQuery, connection))
                {
                    selectMinion.Parameters.AddWithValue("@Name", minionName);

                    int? result = (int?)selectMinion.ExecuteScalar();

                    if (result == null)
                    {
                        string insertQuery = "INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";

                        using (SqlCommand insertVillain = new SqlCommand(insertQuery, connection))
                        {
                            insertVillain.Parameters.AddWithValue("@nam", minionName);
                            insertVillain.Parameters.AddWithValue("@age", minionAge);
                            insertVillain.Parameters.AddWithValue("@townId", townID);

                            int rows = insertVillain.ExecuteNonQuery();

                            minionID = (int?)selectMinion.ExecuteScalar();
                        }
                    }
                    else
                    {
                        minionID = result;
                    }

                    string insertMinionToVillainQuery = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

                    using (SqlCommand insertMinionToVillain = new SqlCommand(insertMinionToVillainQuery, connection))
                    {
                        insertMinionToVillain.Parameters.AddWithValue("@villainId", villainID);
                        insertMinionToVillain.Parameters.AddWithValue("@minionId", minionID);

                        int rows = insertMinionToVillain.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                        }
                    }
                }
            }
        }
    }
}
