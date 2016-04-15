using GHIElectronics.UWP.Shields;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using Windows.ApplicationModel.Background;


namespace IoTHubMqttClient {
    public sealed class StartupTask : IBackgroundTask {
        BackgroundTaskDeferral deferral;

        private FEZHAT hat;

        Telemetry telemetry = new Telemetry("Sydney");

        // https://azure.microsoft.com/en-us/documentation/articles/iot-hub-mqtt-support/
        const string hubAddress = "MakerDen.azure-devices.net";
        const string hubName = "MqttDevice";
        const string hubPass = "SharedAccessSignature sr=MakerDen.azure-devices.net%2fdevices%2fMqttDevice&sig=sh8ZVK3L4u9XYuTeI%2f3l%2bx7X6D3BRJADz1rppuK3hvw%3d&se=1477701103";

        string hubUser = $"{hubAddress}/{hubName}";
        string hubTopicPublish = $"devices/{hubName}/messages/events/";
        string hubTopicSubscribe = $"devices/{hubName}/messages/devicebound/#";

        private MqttClient client;

        public async void Run(IBackgroundTaskInstance taskInstance) {
            deferral = taskInstance.GetDeferral();

            this.hat = await FEZHAT.CreateAsync();

            // https://m2mqtt.wordpress.com/m2mqtt_doc/
            client = new MqttClient(hubAddress, 8883, true, MqttSslProtocols.TLSv1_2);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.Subscribe(new string[] { hubTopicSubscribe }, new byte[] { 0 });

            var result = Task.Run(async () => {

                while (true) {

                    if (!client.IsConnected) { client.Connect(hubName, hubUser, hubPass); }
                    if (client.IsConnected) {
                        client.Publish(hubTopicPublish, telemetry.ToJson(hat.GetTemperature(), hat.GetLightLevel(), 50)); // no build in humidity sensor - just assume 50%
                    }
                    await Task.Delay(60000); // don't leave this running for too long at this rate as you'll quickly consume your free daily Iot Hub Message limit
                }
            });
        }

        private void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e) {
            string message = System.Text.Encoding.UTF8.GetString(e.Message).ToUpperInvariant();
            Debug.WriteLine($"Command Received: {message}");

            switch (message) {
                case "RED":
                    hat.D2.Color = new FEZHAT.Color(255, 0, 0);
                    break;
                case "GREEN":
                    hat.D2.Color = new FEZHAT.Color(0, 255, 0);
                    break;
                case "BLUE":
                    hat.D2.Color = new FEZHAT.Color(0, 0, 255);
                    break;
                case "OFF":
                    hat.D2.TurnOff();
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Unrecognized command: {0}", message);
                    break;
            }
        }
    }
}
