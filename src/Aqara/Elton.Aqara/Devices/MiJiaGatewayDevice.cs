using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class MiJiaGatewayDevice : AqaraDevice
    {
        public MiJiaGatewayDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config) : base(connector, gateway, sid, config)
        {
        }

        private Color ParseColor(dynamic color)
        {
            Color result = default(Color);

            try
            {
                if (color is uint)
                {
                    var a = (color & 0xFF000000) >> 24;
                    var r = (color & 0x00FF0000) >> 16;
                    var g = (color & 0x0000FF00) >> 8;
                    var b = (color & 0x000000FF);
                    result = Color.FromArgb((int)a, (int)r, (int)g, (int)b);
                }
                else if (color is int)
                {
                    color = Convert.ToUInt32(color);
                    var a = (color & 0xFF000000) >> 24;
                    var r = (color & 0x00FF0000) >> 16;
                    var g = (color & 0x0000FF00) >> 8;
                    var b = (color & 0x000000FF);
                    result = Color.FromArgb(a, r, g, b);
                }
                else if (color is string)
                {
                    try
                    {
                        result = ColorTranslator.FromHtml(color);
                        if (result == Color.Empty)
                        {
                            if (!color.StartsWith("#")) color = $"#{color}";
                            if (color.Length == 7) color.Insert(1, "ff");
                            else if (color.Length <= 9)
                            {

                                if (string.IsNullOrEmpty(color)) result = ColorTranslator.FromHtml($"#{lastcolor:X}");
                                else result = ColorTranslator.FromHtml(color);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        result = Color.FromName(color);
                    }
                }
                else if (color is Color)
                {
                    result = color;
                }
            }
            catch (Exception) { }

            return (result == Color.Empty ? default(Color) : result);
        }

        private uint ParseColorValue(dynamic color)
        {
            uint result = default(uint);

            try
            {
                if (color is uint)
                {
                    result = color;
                }
                else if (color is int)
                {
                    result = Convert.ToUInt32(color);
                }
                else if (color is string)
                {
                    Color c = default(Color);
                    try
                    {
                        c = ColorTranslator.FromHtml(color);
                        if (c == Color.Empty)
                        {
                            if (!color.StartsWith("#")) color = $"#{color}";
                            if (color.Length == 7) color.Insert(1, "ff");
                            else if (color.Length <= 9)
                            {
                                if (string.IsNullOrEmpty(color)) c = ColorTranslator.FromHtml($"#{lastcolor:X}");
                                else c = ColorTranslator.FromHtml(color);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        c = Color.FromName(color);
                    }
                    if (c != Color.Empty)
                        result = (uint)c.A << 24 | (uint)c.R << 16 | (uint)c.G << 8 | (uint)c.B;
                }
                else if (color is Color)
                {
                    result = color.A << 24 | color.R << 16 | color.G << 8 | color.B;
                }
            }
            catch (Exception) { }

            return (result);
        }

        private uint lastcolor = 0;
        private uint rgb = 0;
        public dynamic Light
        {
            get
            {
                Color result = Color.White;
                try
                {
                    if (States.ContainsKey("rgb"))
                    {
                        try
                        {
                            rgb = Convert.ToUInt32(States["rgb"].Value);
                            result = ParseColor(rgb);
                            //var a = (rgb & 0xFF000000) >> 24; // 0xFF000000;
                            //var r = (rgb & 0x00FF0000) >> 16;
                            //var g = (rgb & 0x0000FF00) >> 8;
                            //var b = (rgb & 0x000000FF);
                            //result = Color.FromArgb((int)a, (int)r, (int)g, (int)b);
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }
                return (result);
            }
            set
            {
                uint c = ParseColorValue(value);
                LightOn(c);
            }
        }

        public bool LightIsOn()
        {
            if (Light == Color.Black) return (false);
            else return (true);
        }

        public void LightOn(uint color = 0)
        {
            try
            {
                if (color == 0) color = lastcolor;
                else lastcolor = color;
                var data = new List<KeyValuePair<string, dynamic>>();
                data.Add(new KeyValuePair<string, dynamic>("rgb", color));
                connector.SendWriteCommand(this, data);
            }
            catch (Exception) { }
        }

        public void LightOn(string color)
        {
            var value = ParseColorValue(color);
            LightOn(value);
        }

        public void LightOn(Color color)
        {
            var value = ParseColorValue(color);
            LightOn(value);
        }

        public void LightOn(dynamic color)
        {
            var value = ParseColorValue(color);
            LightOn(value);
        }

        public void LightOff()
        {
            try
            {
                var data = new List<KeyValuePair<string, dynamic>>();
                data.Add(new KeyValuePair<string, dynamic>("rgb", 0));
                connector.SendWriteCommand(this, data);
            }
            catch (Exception) { }
        }

        public uint Lux
        {
            get
            {
                uint result = 0;
                try
                {
                    if (States.ContainsKey("illumination"))
                    {
                        try
                        {
                            result = Convert.ToUInt32(States["illumination"].Value);
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        public void Play(int idx)
        {
            Stop();
            var data = new List<KeyValuePair<string, dynamic>>();
            data.Add(new KeyValuePair<string, dynamic>("mid", idx));
            connector.SendWriteCommand(this, data);
        }

        public void Stop()
        {
            var data = new List<KeyValuePair<string, dynamic>>();
            data.Add(new KeyValuePair<string, dynamic>("mid", 10000));
            connector.SendWriteCommand(this, data);
        }

        private string new_value;
        public override string NewStateValue
        {
            get { return (new_value); }
            set
            {
                new_value = value;
                if (NewStateName.Equals("rgb"))
                {
                    try
                    {
                        rgb = Convert.ToUInt32(States["rgb"].Value);
                        if (rgb != 0) lastcolor = rgb;
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
