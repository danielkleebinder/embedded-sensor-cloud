using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace MyWebServer.Data.Tests
{
    [TestFixture]
    public class Custom_DatabaseAccessTests
    {

        #region Helper
        protected DatabaseAccess CreateDatabaseAccess()
        {
            DatabaseAccess dao = new VirtualDatabaseAccess();
            dao.Initialize();
            return dao;
        }
        #endregion

        #region Database Access Object
        [Test]
        public void database_access_initialize()
        {
            // Should be created successfully without throwing any
            // initialization exceptions
            DatabaseAccess dao = CreateDatabaseAccess();
        }

        [Test]
        public void database_access_save_temperatures()
        {
            DatabaseAccess dao = CreateDatabaseAccess();

            // Create VALID temperature objects
            for (int i = 0; i < 10; i++)
            {
                // Save the temperature objects without exceptions
                dao.SaveTemperature(new Temperature(new DateTime(i), i));
            }
        }

        [Test]
        public void database_access_load_all_temperatures()
        {
            DatabaseAccess dao = CreateDatabaseAccess();

            // Create VALID temperature objects
            List<Temperature> temps = new List<Temperature>(17);
            for (int i = 0; i < 10; i++)
            {
                Temperature current = new Temperature(new DateTime(i), i);
                temps.Add(current);

                // Save the temperature objects without exceptions
                dao.SaveTemperature(current);
            }

            // Load all temperatures
            CollectionAssert.AreEquivalent(temps, dao.LoadAllTemperatures());
        }

        [Test]
        public void database_access_load_temperature_by_id()
        {
            DatabaseAccess dao = CreateDatabaseAccess();

            // Check if the temperature can be loaded using its ID
            dao.SaveTemperature(new Temperature(new DateTime(0), 33.7));
            Temperature expected = dao.LoadAllTemperatures()[0];
            Temperature loaded = dao.LoadTemperature(expected.ID);
            Assert.AreEqual(expected.ID, loaded.ID);
            Assert.AreEqual(expected.Date, loaded.Date);
            Assert.AreEqual(expected.Celsius, loaded.Celsius);
        }

        [Test]
        public void database_access_load_all_temperatures_in_range()
        {
            DatabaseAccess dao = CreateDatabaseAccess();

            // Create VALID temperature objects
            List<Temperature> temps = new List<Temperature>(17);
            for (int i = 0; i < 10; i++)
            {
                Temperature current = new Temperature(new DateTime(i), i);
                temps.Add(current);

                // Save the temperature objects without exceptions
                dao.SaveTemperature(current);
            }

            // Check if all four temperatures in range are returned
            Assert.AreEqual(4, dao.LoadAllTemperaturesRange(new DateTime(2), new DateTime(5)).Count);
        }

        [Test]
        public void database_access_save_delete()
        {
            DatabaseAccess dao = CreateDatabaseAccess();

            // Save and delete temperature without exceptions
            Temperature temp = new Temperature();
            dao.SaveTemperature(temp);
            dao.DeleteTemperature(temp);
        }

        [Test]
        public void database_access_delete_temperature()
        {
            DatabaseAccess dao = CreateDatabaseAccess();

            // This should throw an exception
            try
            {
                Temperature temp = new Temperature();
                temp.ID = -1;
                dao.DeleteTemperature(temp);
            }
            catch
            {
                Assert.Pass("Database access object threw an exception");
            }

            // Fail is no exception was thrown
            Assert.Fail("Database access object should throw an exception if the given temperature ID is below 0!");
        }
        #endregion
    }
}