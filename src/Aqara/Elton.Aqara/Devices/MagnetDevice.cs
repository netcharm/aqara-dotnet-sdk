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

        private int no_close = 0;
        public int NoClosed
        {
            get
            {
                try
                {
                    if (NewStateName.Equals("no_close")) no_close = Convert.ToInt32(NewStateValue);
                    else if (NewStateName.Equals("status") && NewStateValue.Equals("open")) no_close = 0;
                    //if (States.ContainsKey("no_close"))
                    //{
                    //    no_close = Convert.ToInt32(States["no_close"].Value);
                    //}
                }
                catch (Exception) { }
                return (no_close);
            }
        }
    }
}
