using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebServer.Data
{
    public class Temperature
    {
        public Temperature() : this(DateTime.Now) { }
        public Temperature(DateTime date) : this(date, 0.0) { }
        public Temperature(double celsius) : this(DateTime.Now, celsius) { }
        public Temperature(DateTime date, double celsius)
        {
            this.Date = date;
            this.Celsius = celsius;
        }

        /// <summary>
        /// Database model values.
        /// </summary>
        public int ID { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public double Celsius { get; set; } = 0.0;


        /// <summary>
        /// Converts the standard celsius temperature to kelvin temperature values.
        /// </summary>
        public double Kelvin
        {
            get
            {
                return Celsius + 273.15;
            }
            set
            {
                Celsius = value - 273.15;
            }
        }


        /// <summary>
        /// Converts the standard celsius temperature to fahrenheit temperature values.
        /// </summary>
        public double Fahrenheit
        {
            get
            {
                return Celsius * 1.8 + 32.0;
            }
            set
            {
                Celsius = (value - 32.0) / 1.8;
            }
        }
    }
}