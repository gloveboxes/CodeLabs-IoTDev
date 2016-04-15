using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTHubMqttClient {
    public sealed class Sensor {

        public Sensor(string guid, string measureName, string unitofmeasure) {
            this.guid = guid;
            this.measurename = measureName;
            this.unitofmeasure = unitofmeasure;
        }

        public string location => "USA";
        public string organisation => "Fabrikam";
        public string guid { get; set; }
        public string measurename { get; set; }
        public string unitofmeasure { get; set;}
        public string value { get; set; }
        public string timecreated { get; set; }

        public byte[] ToJSON(double measurement) {
            value = measurement.ToString();
            timecreated = DateTime.UtcNow.ToString("o");     
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
}
