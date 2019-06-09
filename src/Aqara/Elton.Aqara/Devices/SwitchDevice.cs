using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    /// <summary>
    /// switch 无线开关传感器
    /// </summary>
    public class SwitchDevice : AqaraDevice
    {
        public SwitchDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config) : base(connector, gateway, sid, config)
        {
        }

        public bool IsClick
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("click", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool IsDoubldClick
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (States["status"].Value.Equals("double_click", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public bool IsLongPress
        {
            get
            {
                bool result = false;
                try
                {
                    if (States.ContainsKey("status") && NewStateName.Equals("status", StringComparison.OrdinalIgnoreCase))
                    {
                        //if (States["status"].Value.Equals("long_click_press", StringComparison.OrdinalIgnoreCase)) result = true;
                        if (States["status"].Value.Equals("long_click_release", StringComparison.OrdinalIgnoreCase)) result = true;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public void Click()
        {
            Write("click");
        }

        public void DoubleClick()
        {
            Write("double_click");
        }

        public void LongPress()
        {
            Write("long_click_press");
            System.Threading.Thread.Sleep(50);
            Write("long_click_release");
        }
    }
}
