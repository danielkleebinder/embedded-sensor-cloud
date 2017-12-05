using System;
using System.Collections.Generic;

/// <summary>
/// The data package contains all model and access classes for database connections.
/// </summary>
namespace MyWebServer.Data
{
    /// <summary>
    /// Basic database access interface for abstract connections to the data models of the application.
    /// </summary>
    public interface DatabaseAccess
    {
        /// <summary>
        /// Initializes the database access. Not all connections need this method!
        /// </summary>
        void Initialize();

        /// <summary>
        /// Saves the given temperature object into the database.
        /// </summary>
        /// <param name="tmp">Temperature to be saved.</param>
        void SaveTemperature(Temperature tmp);

        /// <summary>
        /// Deletes the given temperature entry from the database.
        /// </summary>
        /// <param name="tmp">Temperature to be deleted.</param>
        void DeleteTemperature(Temperature tmp);


        /// <summary>
        /// Loads the temperature entry with the given ID from the database.
        /// </summary>
        /// <param name="id">Temperature database ID (Primary Key)</param>
        /// <returns>Temperature object from the database with the given ID or null if no entry was found.</returns>
        Temperature LoadTemperature(int id);

        /// <summary>
        /// Loads all temperature entries from the database.
        /// </summary>
        /// <returns>All temperature entries from the database.</returns>
        List<Temperature> LoadAllTemperatures();

        /// <summary>
        /// Loads all temperature entries from the database which are in the given
        /// date time range.
        /// </summary>
        /// <param name="from">Starting date.</param>
        /// <param name="until">End date.</param>
        /// <returns>Temperatures in the given range.</returns>
        List<Temperature> LoadAllTemperaturesRange(DateTime from, DateTime until);
    }
}
