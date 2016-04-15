# Controlling a device from Azure IoT Hub

Azure IoT Hub is a service that enables reliable and secure bi-directional communications between millions of IoT devices and an application back end. In this section you will see how to send cloud-to-device messages to your device to command it to change the color of one of the FEZ HAT leds, using the Device Explorer app as the back end.

Azure IoT Hub also supports a number of protocols including [AMQP](https://en.wikipedia.org/wiki/AMPQ), HTTPS and [MQTT](https://en.wikipedia.org/wiki/MQTT).

In this lab we are going to publish data to Azure IoT Hub over MQTT and subscript to command messages.

We will use two NuGet Packages: -

1. The [M2Mqtt Client Library](https://m2mqtt.wordpress.com/using-mqttclient)
2. The [Fez Hat Library](https://www.ghielectronics.com/docs/329/fez-hat-developers-guide)

### Create a Headless Windows IoT Core Application

Visual Studio -> New Project -> Windows IoT Core

![Create New IoT Core Background Application](/images/mqtt-background-application-new.png?raw=true)

![New Universal Project Defaults](/images/mqtt-new-universal-project-defaults.png?raw=true)

Install-Package M2Mqtt 
Install-Package Newtonsoft.Json 
Install-Package GHIElectronics.UWP.Shields.FEZHAT


##Completed

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

            Sensor temperature = new Sensor("41c2e437-6c3d-48d0-8e12-81eab2aa5013", "Temperature", "C");
            Sensor light = new Sensor("41c2e437-6c3d-48d0-8e12-81eab2aa5013", "Light", "L");

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
                client.Connect(hubName, hubUser, hubPass);

                var result = Task.Run(async () => {

                    while (true) {

                        if (!client.IsConnected) { client.Connect(hubName, hubUser, hubPass); }
                        if (client.IsConnected) {
                            client.Publish(hubTopicPublish, temperature.ToJson(hat.GetTemperature()));
                            client.Publish(hubTopicPublish, light.ToJson(hat.GetLightLevel()));
                        }
                        await Task.Delay(20000); // don't leave this running for too long at this rate as you'll quickly consume your free daily Iot Hub Message limit
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
