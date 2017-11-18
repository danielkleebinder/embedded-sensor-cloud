using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWebServer.Data
{
    public class VirtualDatabaseAccess : DatabaseAccess
    {
        public void Initialize()
        {
        }

        public void DeleteTemperature(Temperature tmp)
        {
        }

        public List<Temperature> LoadAllTemperatures()
        {
            return new List<Temperature>();
        }

        public List<Temperature> LoadAllTemperaturesSection(DateTime from, DateTime until)
        {
            return new List<Temperature>();
        }

        public Temperature LoadTemperature()
        {
            return new Temperature();
        }

        public void SaveTemperature(Temperature tmp)
        {
        }
    }
}
