﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class AqaraDevice
    {
        protected readonly AqaraClient connector = null;
        protected readonly AqaraGateway gateway = null;
        protected readonly AqaraDeviceConfig config = null;

        readonly Dictionary<string, DeviceState> dicStates = new Dictionary<string, DeviceState>(StringComparer.OrdinalIgnoreCase);

        DateTime latestTimestamp = DateTime.MinValue;
        public string Id { get; private set; }
        public ushort ShortId { get; private set; }
        public AqaraDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config)
        {
            this.connector = connector;
            this.gateway = gateway;
            this.Id = sid;
            this.config = config;
            this.model = DeviceModel.GetModelByName(config.Model);
            description = DeviceModel.GetDescription(config.Model);
        }

        public string Name
        {
            get
            {
                if (config == null)
                    return name;

                return config.Name;
            }
            set { name = value; }
        }

        public DeviceModel Model => model;

        string name = default(string);
        DeviceModel model = null;
        string description = default(string);
        public virtual void Update(string modelName, long short_id, string jsonString=default(string))
        {
            model = DeviceModel.Parse(modelName);
            if (short_id > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("short_id 值比 UInt16 大。");
            ShortId = (ushort)short_id;

            latestTimestamp = DateTime.Now;
        }

        public List<StateChangedEventArgs> UpdateData(string jsonString)
        {
            var listChanged = new List<StateChangedEventArgs>();

            dynamic data = JsonConvert.DeserializeObject(jsonString);
            foreach (var item in data)
            {
                string stateName = item.Name;
                string stateData = item.Value;

                if (!dicStates.ContainsKey(stateName))
                    dicStates.Add(stateName, new DeviceState(stateName));

                var state = dicStates[stateName];

                listChanged.Add(new StateChangedEventArgs(this, stateName,
                    oldData: state.IsUnknown ? null : state.Value,
                    newData: stateData));

                if (state.IsUnknown || state.Value != stateData)
                    state.SetValue(stateData);
            }
            latestTimestamp = DateTime.Now;

            return listChanged;
        }

        public AqaraGateway Gateway
        {
            get { return gateway; }
        }

        public AqaraDeviceConfig Config => config;

        /// <summary>
        /// 设备属性。
        /// </summary>
        public Dictionary<string, DeviceState> States
        {
            get { return dicStates; }
        }

        public DateTime LatestTimestamp
        {
            get { return latestTimestamp; }
        }

        public virtual string NewStateName { get; set; } = string.Empty;
        public virtual string NewStateValue { get; set; } = string.Empty;
        public virtual uint StateDuration { get; set; } = 0;

        public double Voltage
        {
            get
            {
                double result = double.NaN;
                try
                {
                    if (States.ContainsKey("voltage"))
                    {
                        result = Convert.ToDouble(States["voltage"].Value) / 1000.0;
                    }
                }
                catch (Exception) { }
                return (result);
            }
        }

        protected void Read()
        {
            connector.SendCommand(this.Gateway, $"{{\"cmd\":\"read\",\"sid\":\"{this.Id}\"}}");
        }

        protected void Write(string status)
        {
            //{"cmd":"write","model":"switch","sid":"112316","short_id":4343,"data":"{\"status\":\"click\"}" }
            var dic = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);
            dic.Add("status", status);
            connector.SendWriteCommand(this, dic);
        }

        protected void Write(string key, string value)
        {
            var dic = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);
            dic.Add(key, value);
            connector.SendWriteCommand(this, dic);
        }

        protected void Write(IEnumerable<KeyValuePair<string, string>> values)
        {
            var dic = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in values)
            {
                dic.Add(kv.Key, kv.Value);
            }
            connector.SendWriteCommand(this, dic);
        }
    }
}
