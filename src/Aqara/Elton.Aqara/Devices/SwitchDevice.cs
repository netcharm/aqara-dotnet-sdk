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
