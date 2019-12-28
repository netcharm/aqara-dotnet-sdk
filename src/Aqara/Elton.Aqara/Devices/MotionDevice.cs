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
        public bool IsMotion { get { return Motion; } }

        private uint no_motion = 0;
        public uint NoMotion
        {
            get
            {
                try
                {
                    if (NewStateName.Equals("status") && NewStateValue.Equals("motion"))
                    {
                        no_motion = 0;
                    }
                }
                catch (Exception) { }
                return (no_motion + StateDuration);
            }
        }

        public uint Lux
        {
            get
            {
                uint result = 0;
                try
                {
                    if (States.ContainsKey("lux"))
                    {
                        try
                        {
                            result = Convert.ToUInt32(States["lux"].Value);
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        private string new_value;
        public override string NewStateValue
        {
            get { return (new_value); }
            set
            {
                new_value = value;
                if (NewStateName.Equals("status") && NewStateValue.Equals("motion")) no_motion = 0;
                else if (NewStateName.Equals("no_motion"))
                {
                    try
                    {
                        no_motion = Math.Max(no_motion, Convert.ToUInt32(value));
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
