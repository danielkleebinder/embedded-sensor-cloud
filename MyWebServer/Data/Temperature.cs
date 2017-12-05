using System;

namespace MyWebServer.Data
{
    /// <summary>
    /// A model class which represents temperatures and is able to convert the temperature
    /// values in different systems.
    /// </summary>
    public class Temperature
    {
        /// <summary>
        /// Creates a new instance of "Temperature" with the the current date time as record value
        /// and 0.0 degrees celsius.
        /// </summary>
        public Temperature() : this(DateTime.Now) { }

        /// <summary>
        /// Creates a new instance of "Temperature" with the given date time as record value
        /// and 0.0 degrees celsius.
        /// </summary>
        /// <param name="date">Date time of record.</param>
        public Temperature(DateTime date) : this(date, 0.0) { }

        /// <summary>
        /// Creates a new instance of "Temperature" with the given temperature in celsius and
        /// the current date time as record value.
        /// </summary>
        /// <param name="celsius">Temperature in celsius.</param>
        public Temperature(double celsius) : this(DateTime.Now, celsius) { }

        /// <summary>
        /// Creates a new instance of "Temperature" with the given date time as record value and the
        /// given temperature in celsius.
        /// </summary>
        /// <param name="date">Date time of record.</param>
        /// <param name="celsius">Temperature in celsius.</param>
        public Temperature(DateTime date, double celsius)
        {
            Date = date;
            Celsius = celsius;
        }

        /// <summary>
        /// Returns and sets the primary key value (ID) of the temperature. This value is
        /// only used for database conventions.
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// Returns and sets the exact date time value when the record was taken and saved.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// Returns and sets the temperature values in celsius.
        /// </summary>
        public double Celsius { get; set; } = 0.0;


        /// <summary>
        /// Converts the standard celsius temperature to kelvin temperature values when
        /// setting or returning the value.
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
        /// Converts the standard celsius temperature to fahrenheit temperature values when
        /// setting or returning the value.
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

        /// <summary>
        /// Overrides the standard equals method for comparison of the temperature class.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns>True if equals, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            var item = obj as Temperature;
            if (item == null)
            {
                return false;
            }

            // Check if ID is the same
            if (!ID.Equals(item.ID))
            {
                return false;
            }

            // Check if date is the same
            if (!Date.Equals(item.Date))
            {
                return false;
            }

            // Check if the temperature is the same
            return Celsius.Equals(item.Celsius);
        }

        /// <summary>
        /// Overrides the standard hash code function and returns an appropriate hash code.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return ID.GetHashCode()
                + Date.GetHashCode()
                + Celsius.GetHashCode();
        }
    }
}