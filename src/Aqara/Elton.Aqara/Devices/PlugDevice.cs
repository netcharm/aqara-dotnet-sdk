using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    /// <summary>
    /// plug 智能插座
    /// </summary>
    public class PlugDevice : AqaraDevice
    {
        public PlugDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config) : base(connector, gateway, sid, config)
        {
        }

        public bool IsOn
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status"))
                    {
                        if (States["status"].Value.Equals("on", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool InUse
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("inuse"))
                    {
                        if (States["inuse"].Value.Equals("1", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public double LoadVoltage
        {
            get
            {
                double result = 0;
                try
                {
                    if (States.ContainsKey("load_voltage"))
                    {
                         result = Convert.ToDouble(States["load_voltage"].Value)/1000;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public double LoadPower
        {
            get
            {
                double result = 0;
                try
                {
                    if (States.ContainsKey("load_power"))
                    {
                        result = Convert.ToDouble(States["load_voltage"].Value);
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public double PowerConsumed
        {
            get
            {
                double result = 0;
                try
                {
                    if (States.ContainsKey("power_consumed"))
                    {
                        result = Convert.ToDouble(States["power_consumed"].Value);
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public void On()
        {
            Write("on");
        }

        public void Off()
        {
            Write("off");
        }

        public void TurnOn()
        {
            On();
        }

        public void TurnOff()
        {
            Off();
        }

        public void Toggle()
        {
            if (IsOn) Off();
            else On();
        }
    }
}
