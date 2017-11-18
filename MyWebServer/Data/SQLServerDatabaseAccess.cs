using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;

namespace MyWebServer.Data
{
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


        public void Initialize()
        {
            // Nothing to do here
        }

        public void SaveTemperature(Temperature tmp)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_URI))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(SQL_STATEMENT_SAVE_TEMPERATURE, connection))
                {
                    command.Parameters.AddWithValue("@date", tmp.Date);
                    command.Parameters.AddWithValue("@celsius", tmp.Celsius);
                    if (command.ExecuteNonQuery() <= 0)
                    {
                        throw new Exception("0 rows where affected by the insertion!");
                    }
                }
            }
        }

        public void DeleteTemperature(Temperature tmp)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_URI))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(SQL_STATEMENT_DELETE_TEMPERATURE, connection))
                {
                    command.Parameters.AddWithValue("@id", tmp.ID);
                }
            }
        }

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

        public List<Temperature> LoadAllTemperaturesSection(DateTime from, DateTime until)
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

        public Temperature LoadTemperature()
        {
            throw new NotImplementedException();
        }
    }
}
