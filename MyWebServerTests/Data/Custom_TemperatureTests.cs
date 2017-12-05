using System;
using NUnit.Framework;

namespace MyWebServer.Data.Tests
{
    [TestFixture]
    public class Custom_TemperatureTests
    {
        [Test]
        public void temperature_conversion_celsius_kelvin()
        {
            Temperature tmp = new Temperature(-273.15);
            Assert.AreEqual(Math.Round(tmp.Kelvin, 2), 0.0);
        }

        [Test]
        public void temperature_conversion_kelvin_celsius()
        {
            Temperature tmp = new Temperature();
            tmp.Kelvin = 0.0;
            Assert.AreEqual(Math.Round(tmp.Celsius, 2), -273.15);
        }

        [Test]
        public void temperature_conversion_celsius_fahrenheit()
        {
            Temperature tmp = new Temperature(0.0);
            Assert.AreEqual(Math.Round(tmp.Fahrenheit, 2), 32.0);
        }

        [Test]
        public void temperature_conversion_fahrenheit_celsius()
        {
            Temperature tmp = new Temperature();
            tmp.Fahrenheit = 5.0;
            Assert.AreEqual(Math.Round(tmp.Celsius, 2), -15.0);
        }

        [Test]
        public void temperature_conversion_fahrenheit_kelvin()
        {
            Temperature tmp = new Temperature();
            tmp.Fahrenheit = 100.0;
            Assert.AreEqual(Math.Round(tmp.Kelvin, 2), 310.93);
        }

        [Test]
        public void temperature_conversion_kelvin_fahrenheit()
        {
            Temperature tmp = new Temperature();
            tmp.Kelvin = 256.0;
            Assert.AreEqual(Math.Round(tmp.Fahrenheit, 2), 1.13);
        }

        [Test]
        public void temperature_set_kelvin_celsius_fahrenheit()
        {
            Temperature tmp = new Temperature();
            tmp.Kelvin = 17.31;
            Assert.AreEqual(Math.Round(tmp.Kelvin, 2), 17.31);
            tmp.Celsius = 23.7;
            Assert.AreEqual(Math.Round(tmp.Celsius, 2), 23.7);
            tmp.Fahrenheit = 94.91;
            Assert.AreEqual(Math.Round(tmp.Fahrenheit, 2), 94.91);
        }

        [Test]
        public void temperature_check_equals()
        {
            Temperature temp1 = new Temperature();
            temp1.ID = 10;
            temp1.Date = new DateTime(2);
            temp1.Kelvin = 100;

            Temperature temp2 = new Temperature();
            temp2.ID = 10;
            temp2.Date = new DateTime(2);
            temp2.Kelvin = 100;

            Temperature temp3 = new Temperature();
            temp3.ID = 11;
            temp3.Date = new DateTime(2);
            temp3.Kelvin = 100;

            Assert.AreEqual(temp1, temp2);
            Assert.AreNotEqual(temp1, temp3);
            Assert.AreNotEqual(temp2, temp3);
        }

        [Test]
        public void temperature_check_hash_code()
        {
            Temperature temp1 = new Temperature();
            temp1.ID = 12;
            temp1.Date = new DateTime(4);
            temp1.Kelvin = 101;

            Temperature temp2 = new Temperature();
            temp2.ID = 12;
            temp2.Date = new DateTime(4);
            temp2.Kelvin = 101;

            Temperature temp3 = new Temperature();
            temp3.ID = 11;
            temp3.Date = new DateTime(2);
            temp3.Kelvin = 100;

            Assert.AreEqual(temp1.GetHashCode(), temp2.GetHashCode());
            Assert.AreNotEqual(temp1.GetHashCode(), temp3.GetHashCode());
            Assert.AreNotEqual(temp2.GetHashCode(), temp3.GetHashCode());
        }

        [Test]
        public void temperature_check_equals_operator()
        {
            Temperature temp1 = new Temperature();
            temp1.ID = 12;
            temp1.Date = new DateTime(4);
            temp1.Kelvin = 101;

            Temperature temp2 = new Temperature();
            temp2.ID = 12;
            temp2.Date = new DateTime(4);
            temp2.Kelvin = 101;

            if (temp1 != temp2)
            {
                Assert.Pass("Equals operator '==' successfully failed");
            }
            Assert.Fail("Equals operator '==' has to fail!");
        }
    }
}