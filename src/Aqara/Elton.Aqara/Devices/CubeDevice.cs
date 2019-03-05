using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class CubeDevice : AqaraDevice
    {
        public CubeDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config) : base(connector, gateway, sid, config)
        {
        }

        public bool Flip90
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("flip90", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool Flip180
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("flip180", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool Move
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("move", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool Tap_Twice
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("tap_twice", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool Shake_Air
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("shake_air", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool Swing
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("swing", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool Alert
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("alert", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool Free_Fall
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("free_fall", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public class RotateValue
        {
            public double Rotate { get; set; }
            public double Duration { get; set; }
        }

        public RotateValue Rotate
        {
            get
            {
                RotateValue result = default(RotateValue);
                try
                {
                    if (States.ContainsKey("rotate") && NewStateName.Equals("rotate", StringComparison.OrdinalIgnoreCase))
                    {
                        var value = States["rotate"].Value.Split(',');
                        if (value.Length == 2)
                        {
                            result.Rotate = Convert.ToDouble(value[0].Trim()) * 3.6;
                            result.Duration = Convert.ToDouble(value[1].Trim());
                        }
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

    }
}
