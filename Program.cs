using System;
using System.Data.SqlClient;
using Npgsql; // Npgsql for PostgreSQL

namespace PostgresToSqlServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // PostgreSQL connection string (replace with your details)
            string postgresConnString = "Host=your_postgres_host;Port=5432;Username=your_pg_username;Password=your_pg_password;Database=your_pg_db";

            // SQL Server connection string (replace with your details)
            string sqlServerConnString = "Server=your_sqlserver_host;Database=your_sqlserver_db;User Id=your_sql_username;Password=your_sql_password;";

            try
            {
                // 1. Connect to PostgreSQL and retrieve data
                using (var pgConn = new NpgsqlConnection(postgresConnString))
                {
                    pgConn.Open();

                    string selectQuery = "SELECT id, name, data_column FROM your_pg_table"; // Replace with your query
                    using (var pgCmd = new NpgsqlCommand(selectQuery, pgConn))
                    {
                        using (var reader = pgCmd.ExecuteReader())
                        {
                            // 2. Connect to SQL Server and insert the retrieved data
                            using (var sqlConn = new SqlConnection(sqlServerConnString))
                            {
                                sqlConn.Open();

                                while (reader.Read())
                                {
                                    int id = reader.GetInt32(0);
                                    string name = reader.GetString(1);
                                    string data = reader.GetString(2);

                                    // SQL Server insert query
                                    string insertQuery = "INSERT INTO your_sql_table (id, name, data_column) VALUES (@id, @name, @data)";
                                    using (var sqlCmd = new SqlCommand(insertQuery, sqlConn))
                                    {
                                        // Add parameters
                                        sqlCmd.Parameters.AddWithValue("@id", id);
                                        sqlCmd.Parameters.AddWithValue("@name", name);
                                        sqlCmd.Parameters.AddWithValue("@data", data);

                                        // Execute the insert command
                                        sqlCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("Data transfer from PostgreSQL to SQL Server completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
