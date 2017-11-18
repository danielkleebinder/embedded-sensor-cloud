﻿using System;
using NUnit.Framework;

namespace MyWebServer.Data.Tests
{
    [TestFixture]
    public class TemperatureTests
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
    }
}