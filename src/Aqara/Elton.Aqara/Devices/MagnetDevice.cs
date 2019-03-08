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

        private uint no_close = 0;
        public uint NoClosed
        {
            get
            {
                try
                {
                    if (NewStateName.Equals("status") && NewStateValue.Equals("open"))
                    {
                        no_close = 0;
                    }
                }
                catch (Exception) { }
                return (no_close + StateDuration);
            }
        }

        private string new_value;
        public override string NewStateValue
        {
            get { return (new_value); }
            set
            {
                new_value = value;
                if (NewStateName.Equals("status") && NewStateValue.Equals("open")) no_close = 0;
                else if (NewStateName.Equals("no_close"))
                {
                    try
                    {
                        no_close = Math.Max(no_close, Convert.ToUInt32(value));
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
