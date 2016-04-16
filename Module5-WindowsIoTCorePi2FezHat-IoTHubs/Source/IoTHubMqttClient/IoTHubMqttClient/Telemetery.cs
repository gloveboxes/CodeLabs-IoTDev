using Newtonsoft.Json;
using Porrey.Uwp.Ntp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTHubMqttClient {
    public sealed class Telemetry {

        static int msgCount = 0;
        static bool NtpInitalised = false;
        static TimeSpan utcOffset;
        static DateTime CorrectedUtcTime => utcOffset == TimeSpan.Zero ? DateTime.UtcNow : DateTime.UtcNow - utcOffset;

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
        public int Id { get; set; }

        public byte[] ToJson(double measurement) {
            value = RoundMeasurement(measurement, 2).ToString();
            timecreated = CorrectedTime().ToString("o");
            Id = ++msgCount;
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }

        DateTime CorrectedTime() { // useful for locations particularly conferences with Raspberry Pi failes to sync time
            try {
                if (NtpInitalised) { return CorrectedUtcTime; }

                NtpClient ntp = new NtpClient();

                var time = ntp.GetAsync("au.pool.ntp.org").Result;
                utcOffset = DateTime.UtcNow.Subtract(((DateTime)time).ToUniversalTime());

                NtpInitalised = true;
            }
            catch { }

            return CorrectedUtcTime;
        }

        private string RoundMeasurement(double value, int places) {
            return Math.Round(value, places).ToString();
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


    }
}
