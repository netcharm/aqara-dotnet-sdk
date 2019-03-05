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
        public void Update(string modelName, long short_id)
        {
            this.model = DeviceModel.Parse(modelName);
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

        public string NewStateName = string.Empty;

        //public abstract bool Opened();

        public DateTime LatestTimestamp
        {
            get { return latestTimestamp; }
        }
    }
}
