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

        private int no_motion = 0;
        public int NoMotion
        {
            get
            {
                try
                {
                    if (NewStateName.Equals("no_motion")) no_motion = Convert.ToInt32(NewStateValue);
                    else if (NewStateName.Equals("status") && NewStateValue.Equals("motion")) no_motion = 0;
                    //if (States.ContainsKey("no_motion"))
                    //{
                    //    no_motion = Convert.ToInt32(States["no_motion"].Value);
                    //}
                }
                catch (Exception) { }
                return (no_motion);
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
