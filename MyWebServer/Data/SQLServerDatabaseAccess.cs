using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;

namespace MyWebServer.Data
{
    /// <summary>
    /// A sql server database access implementation.
    /// </summary>
    public class SQLServerDatabaseAccess : DatabaseAccess
    {
        // Connection URI
        private static readonly string CONNECTION_URI =
                "user id=admin;"
                + "password=admin;"
                + "server=localhost;"
                + "Trusted_Connection=yes;"
                + "database=TemperatureSWE;"
                + "connection timeout=30";

        // SQL Statements
        private static readonly string SQL_STATEMENT_SAVE_TEMPERATURE = "Insert Into [Temperature] ([Date], [Celsius]) Values(@date, @celsius)";
        private static readonly string SQL_STATEMENT_DELETE_TEMPERATURE = "Delete From [Temperature] Where [ID] = @id";
        private static readonly string SQL_STATEMENT_ALL_TEMPERATURES = "Select [ID], [Date], [Celsius] From [Temperature]";
        private static readonly string SQL_STATEMENT_ALL_TEMPERATURES_SECTION = "Select [ID], [Date], [Celsius] From [Temperature] Where [Date] >= @from And [Date] <= @until";
        private static readonly string SQL_STATEMENT_LOAD_TEMPERATURE = "Select [ID], [Date], [Celsius] From [Temperature] Where [ID] = @id";

        /// <summary>
        /// Initializes the sql server database access.
        /// </summary>
        public void Initialize()
        {
            // Nothing to do here
        }

        /// <summary>
        /// Saves the given temperature into the database.
        /// </summary>
        /// <param name="tmp">Temperature.</param>
        public void SaveTemperature(Temperature tmp)
        {
            // Create sql connection
            using (SqlConnection connection = new SqlConnection(CONNECTION_URI))
            {
                // Open the connection
                connection.Open();

                // Create sql command
                using (SqlCommand command = new SqlCommand(SQL_STATEMENT_SAVE_TEMPERATURE, connection))
                {
                    // Set parameters
                    command.Parameters.AddWithValue("@date", tmp.Date);
                    command.Parameters.AddWithValue("@celsius", tmp.Celsius);

                    // Check if execution was successful
                    if (command.ExecuteNonQuery() <= 0)
                    {
                        throw new Exception("0 rows where affected by the insertion!");
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the given temperature from the database.
        /// </summary>
        /// <param name="tmp">Temperature.</param>
        public void DeleteTemperature(Temperature tmp)
        {
            if (tmp.ID < 0)
            {
                throw new ArgumentOutOfRangeException("Temperature ID can't be below 0!");
            }

            using (SqlConnection connection = new SqlConnection(CONNECTION_URI))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(SQL_STATEMENT_DELETE_TEMPERATURE, connection))
                {
                    command.Parameters.AddWithValue("@id", tmp.ID);
                }
            }
        }

        /// <summary>
        /// Loads all temperatures from the database.
        /// </summary>
        /// <returns>All temperatures.</returns>
        public List<Temperature> LoadAllTemperatures()
        {
            List<Temperature> result = new List<Temperature>();
            using (SqlConnection connection = new SqlConnection(CONNECTION_URI))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(SQL_STATEMENT_ALL_TEMPERATURES, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Temperature tmp = new Temperature();
                            tmp.ID = (int) reader["ID"];
                            tmp.Date = (DateTime) reader["Date"];
                            tmp.Celsius = (double) reader["Celsius"];
                            result.Add(tmp);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Loads all temperatures within the given date time range from the database.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="until">Until.</param>
        /// <returns>All temperatures in between.</returns>
        public List<Temperature> LoadAllTemperaturesRange(DateTime from, DateTime until)
        {
            List<Temperature> result = new List<Temperature>();
            using (SqlConnection connection = new SqlConnection(CONNECTION_URI))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(SQL_STATEMENT_ALL_TEMPERATURES_SECTION, connection))
                {
                    command.Parameters.AddWithValue("@from", from);
                    command.Parameters.AddWithValue("@until", until);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Temperature tmp = new Temperature();
                            tmp.ID = (int) reader["ID"];
                            tmp.Date = (DateTime) reader["Date"];
                            tmp.Celsius = (double) reader["Celsius"];
                            result.Add(tmp);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Loads the temperature with the given ID from the database.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <returns>Temperature.</returns>
        public Temperature LoadTemperature(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("Temperature ID can't be below 0!");
            }

            // Read temperature entry
            using (SqlConnection connection = new SqlConnection(CONNECTION_URI))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(SQL_STATEMENT_LOAD_TEMPERATURE, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Temperature tmp = new Temperature();
                            tmp.ID = (int) reader["ID"];
                            tmp.Date = (DateTime) reader["Date"];
                            tmp.Celsius = (double) reader["Celsius"];
                            return tmp;
                        }
                    }
                }
            }
            return null;
        }
    }
}