using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class CtrlNeutral2Device : AqaraDevice
    {
        public CtrlNeutral2Device(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config) : base(connector, gateway, sid, config)
        {
        }

        public void Switch(int channel_0, int channel_1)
        {
            var dic = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);
            var c0 = channel_0 > 0 ? "on" : "off";
            var c1 = channel_1 > 0 ? "on" : "off";
            if (channel_0 >= 0)
                dic.Add("channel_0", c0);
            if (channel_1 >= 0)
                dic.Add("channel_1", c1);
            connector.SendWriteCommand(this, dic);
        }

        public void TurnOn()
        {
            Switch(1, 1);
        }

        public void TurnOn(int channel)
        {
            if (channel == 0)
                Switch(1, -1);
            else if (channel == 1)
                Switch(-1, 1);
        }

        public void TurnOff()
        {
            Switch(0, 0);
        }

        public void TurnOff(int channel)
        {
            if (channel == 0)
                Switch(0, -1);
            else if (channel == 1)
                Switch(-1, 0);
        }

    }
}
