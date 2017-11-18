using System;
using System.Collections.Generic;

namespace MyWebServer.Data
{
    public interface DatabaseAccess
    {
        void Initialize();

        void SaveTemperature(Temperature tmp);
        void DeleteTemperature(Temperature tmp);

        Temperature LoadTemperature();
        List<Temperature> LoadAllTemperatures();
        List<Temperature> LoadAllTemperaturesSection(DateTime from, DateTime until);
    }
}
