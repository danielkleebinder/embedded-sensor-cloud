using System;
using System.Collections.Generic;

namespace MyWebServer.Data
{
    /// <summary>
    /// A virtual database access implementation for testing without a backend.
    /// </summary>
    public class VirtualDatabaseAccess : DatabaseAccess
    {
        private static int currentID = 0;
        private Dictionary<int, Temperature> data;

        /// <summary>
        /// Initializes the pseudo database access.
        /// </summary>
        public void Initialize()
        {
            data = new Dictionary<int, Temperature>();
        }

        /// <summary>
        /// Deletes the given temperature.
        /// </summary>
        /// <param name="tmp">Temperature.</param>
        public void DeleteTemperature(Temperature tmp)
        {
            if (tmp.ID < 0)
            {
                throw new ArgumentOutOfRangeException("Temperature ID can't be below 0!");
            }
            data.Remove(tmp.ID);
        }

        /// <summary>
        /// Loads and returns all temperatures.
        /// </summary>
        /// <returns>All temperatures.</returns>
        public List<Temperature> LoadAllTemperatures()
        {
            List<Temperature> result = new List<Temperature>(data.Count);
            foreach (var entry in data)
            {
                result.Add(entry.Value);
            }
            return result;
        }

        /// <summary>
        /// Loads all temperatures within the given date time range.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="until">Until.</param>
        /// <returns>All temperatures in between.</returns>
        public List<Temperature> LoadAllTemperaturesRange(DateTime from, DateTime until)
        {
            List<Temperature> result = new List<Temperature>();
            foreach (var entry in data)
            {
                if (entry.Value.Date >= from && entry.Value.Date <= until)
                {
                    result.Add(entry.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// Loads the temperature with the given ID.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <returns>Temperature.</returns>
        public Temperature LoadTemperature(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("Temperature ID can't be below 0!");
            }
            return data[id];
        }

        /// <summary>
        /// Saves the given temperature.
        /// </summary>
        /// <param name="tmp">Temperature.</param>
        public void SaveTemperature(Temperature tmp)
        {
            if (tmp.ID <= 0)
            {
                tmp.ID = currentID++;
            }
            data.Add(tmp.ID, tmp);
        }
    }
}