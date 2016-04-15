using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTHubMqttClient {
    public sealed class Telemetry {
        public Telemetry(string guid, string measureName, string unitofmeasure) {
            this.guid = guid;
            this.measurename = measureName;
            this.unitofmeasure = unitofmeasure;
        }

        public string location => "USA";
        public string organisation => "Fabrikam";
        public string guid { get; set; }
        public string measurename { get; set; }
        public string unitofmeasure { get; set; }
        public string value { get; set; }
        public string timecreated { get; set; }

        public byte[] ToJson(double measurement) {
            value = measurement.ToString();
            timecreated = DateTime.UtcNow.ToString("o");
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }

        //public Telemetry(string geo) {
        //    this.Geo = geo;
        //}

        //public string Geo { get; set; }
        //public string Celsius { get; set; }
        //public string Humidity { get; set; }
        //public string HPa { get; set; }
        //public string Light { get; set; }
        //public string Utc { get; set; }
        //public int Id { get; set; }


        //public byte[] ToJson(double celcius, double light, double humidity, double hPa) {
        //    Celsius = RoundMeasurement(celcius, 2);
        //    Light = RoundMeasurement(light, 2).ToString();  
        //    Humidity = RoundMeasurement(humidity, 2).ToString();
        //    HPa = RoundMeasurement(hPa, 0).ToString();
        //    Utc = DateTime.UtcNow.ToString("o");
        //    Id++;
        //    return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        //}

        //private string RoundMeasurement(double value, int places) {
        //    return Math.Round(value, places).ToString();
        //}
    }
}
