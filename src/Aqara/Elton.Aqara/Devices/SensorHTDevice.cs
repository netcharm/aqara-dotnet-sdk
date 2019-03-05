using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class SensorHTDevice : AqaraDevice
    {
        public SensorHTDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config) : base(connector, gateway, sid, config)
        {
        }

        public double Temperature
        {
            get
            {
                double result = double.NaN;
                try
                {
                    if (States.ContainsKey("temperature"))
                    {
                        result = Convert.ToDouble(States["temperature"].Value) / 100.0;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public double Humidity
        {
            get
            {
                double result = double.NaN;
                try
                {
                    if (States.ContainsKey("humidity"))
                    {
                        result = Convert.ToDouble(States["humidity"].Value) / 100.0;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public double Pressure
        {
            get
            {
                double result = double.NaN;
                try
                {
                    if (States.ContainsKey("pressure"))
                    {
                        result = Convert.ToDouble(States["pressure"].Value) / 100.0;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }
    }
}
