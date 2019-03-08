using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    /// <summary>
    /// 
    /// </summary>
    public class AqaraGateway
    {
        static readonly Common.Logging.ILog log = Common.Logging.LogManager.GetLogger(typeof(AqaraGateway));

        const int REMOTE_PORT = 9898;

        readonly AqaraClient client = null;
        readonly string sid = null;
        readonly string password = null;
        public string Model { get; internal set; } = string.Empty;
        public string Name { get; internal set; } = string.Empty;
        IPEndPoint endpoint = null;
        string token = null;
        DateTime latestTimestamp = DateTime.MinValue;
        readonly Dictionary<string, AqaraDevice> dicDevices = new Dictionary<string, AqaraDevice>(StringComparer.OrdinalIgnoreCase);
        public AqaraGateway(AqaraClient client, string sid, string password, AqaraDeviceConfig[] devices)
        {
            this.client = client;
            this.sid = sid;
            this.password = password;
            if(devices != null)
            {
                foreach(var item in devices)
                {
                    string systemid = item.DeviceId;
                    if (systemid.StartsWith("lumi.", StringComparison.OrdinalIgnoreCase))
                        systemid = systemid.Substring(5);

                    if (dicDevices.ContainsKey(systemid))
                        throw new ArgumentException("设备清单中已存在具有相同键的元素");

                    //var device = new AqaraDevice(client, this, systemid, item);

                    AqaraDevice device = new AqaraDevice(client, this, systemid, item);
                    if (device.Model is DeviceModel)
                    {
                        switch (device.Model.Name)
                        {
                            case "cube"://a.窗磁传感器
                                device = new CubeDevice(client, this, systemid, item);
                                break;
                            case "magnet"://a.窗磁传感器
                                device = new MagnetDevice(client, this, systemid, item);
                                break;
                            case "motion"://人体传感器
                                device = new MotionDevice(client, this, systemid, item);
                                break;
                            case "switch"://无线开关传感器
                                device = new SwitchDevice(client, this, systemid, item);
                                break;
                            case "plug"://智能插座
                                device = new PlugDevice(client, this, systemid, item);
                                break;
                            case "ctrl_neutral1"://单火开关单键
                                break;
                            case "ctrl_neutral2"://单火开关双键
                                device = new CtrlNeutral2Device(client, this, systemid, item);
                                break;
                            case "86sw1"://无线开关单键
                                break;
                            case "86sw2"://无线开关双键
                                break;
                            case "sensor_ht"://温湿度传感器
                                device = new SensorHTDevice(client, this, systemid, item);
                                break;
                            case "rgbw_light"://j.LUMI.LIGHT.RGBW
                                break;
                            case "gateway"://MiJia/XiaoMi/Aqara Gateway
                                device = new MiJiaGatewayDevice(client, this, systemid, item);
                                break;
                            default:
                                break;
                        }
                    }
                    device.Name = item.Name;
                    dicDevices.Add(systemid, device);
                }
            }
        }

        public void UpdateEndPoint(string remoteIp, int? port = null)
        {
            IPAddress address;
            if (!IPAddress.TryParse(remoteIp, out address))
            {
                log.ErrorFormat("remoteIp format error. (remoteIp='{0}')", remoteIp);
                return;
            }
            if(port != null && port != REMOTE_PORT)
                log.WarnFormat("The remote port is {0}, but the default port is {1} .", port.Value, REMOTE_PORT);

            bool updated = false;
            if (endpoint == null)
            {
                endpoint = new IPEndPoint(address, (port == null) ? 9898 : port.Value);
                updated = true;
            }
            else
            {
                if (endpoint.Address != address)
                {
                    endpoint.Address = address;
                    updated = true;
                }
                if (port != null && endpoint.Port != port.Value)
                {
                    endpoint.Port = port.Value;
                    updated = true;
                }
            }

            if(updated)
                log.InfoFormat("Gateway endpoint was updated, sid='{0}', endpoint='{1}' .", sid, endpoint);
        }
        public void UpdateToken(string token)
        {
            this.token = token;
            latestTimestamp = DateTime.Now;
        }

        public void Update(string remoteIp, string token)
        {
            UpdateEndPoint(remoteIp, null);
            UpdateToken(token);
        }

        public string Id
        {
            get { return this.sid; }
        }

        public string Password
        {
            get { return this.password; }
        }

        /// <summary>
        /// Remote IP Address.
        /// </summary>
        public IPEndPoint EndPoint
        {
            get { return endpoint; }
        }

        public string Token
        {
            get { return token; }
        }

        public DateTime LatestTimestamp
        {
            get { return latestTimestamp; }
        }

        public Dictionary<string, AqaraDevice> Devices => dicDevices;

        public override string ToString()
        {
            return string.Format("GATEWAY[{0}] {1}", sid, EndPoint);
        }
    }
}
