using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class MagnetDevice : AqaraDevice
    {
        public MagnetDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config) : base(connector, gateway, sid, config)
        {
        }

        public bool Opened
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("open", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }

                return (result);
            }
        }

        public bool Closed
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("close", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public int NoClosed
        {
            get
            {
                int result = -1;
                try
                {
                    if (States.ContainsKey("no_close"))
                    {
                        result = Convert.ToInt32(States["no_close"].Value);
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }
    }
}
