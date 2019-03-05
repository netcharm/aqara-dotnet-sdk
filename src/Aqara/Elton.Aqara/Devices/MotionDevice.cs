using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class MotionDevice : AqaraDevice
    {
        public MotionDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config) : base(connector, gateway, sid, config)
        {
        }

        public bool StateChanged { get; set; } = true;

        public bool Motion
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("motion", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public int NoMotion
        {
            get
            {
                int result = -1;
                try
                {
                    if (States.ContainsKey("no_motion"))
                    {
                        result = Convert.ToInt32(States["no_motion"].Value);
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public int Lux
        {
            get
            {
                int result = -1;
                try
                {
                    if (States.ContainsKey("lux"))
                    {
                        result = Convert.ToInt32(States["lux"].Value);
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

    }
}
