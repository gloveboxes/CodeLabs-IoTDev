>[Home](README.md) </br>
>Previous Lab [Analytics with Power Bi](AnalyticsWithPowerBi.md)

# Controlling a device from Azure IoT Hub

Azure IoT Hub is a service that enables reliable and secure bi-directional communications between millions of IoT devices and an application back end. In this section you will see how to send cloud-to-device messages to your device to command it to change the color of one of the FEZ HAT leds, using the Device Explorer app as the back end.

Azure IoT Hub also supports a number of protocols including [AMQP](https://en.wikipedia.org/wiki/AMPQ), HTTPS and [MQTT](https://en.wikipedia.org/wiki/MQTT).

In this lab we are going to publish data to Azure IoT Hub over MQTT and subscript to command messages.

We will use two NuGet Packages: -

1. The [M2Mqtt Client Library](https://m2mqtt.wordpress.com/using-mqttclient)
2. The [Fez Hat Library](https://www.ghielectronics.com/docs/329/fez-hat-developers-guide)

### Create a Headless Windows IoT Core Application

Visual Studio -> New Project -> Windows IoT Core

![Create New IoT Core Background Application](Images/mqtt-background-application-new.png?raw=true)

Accept Universal Windows Application defaults and click Ok

![New Universal Project Defaults](Images/mqtt-new-universal-project-defaults.png?raw=true)



- Install-Package M2Mqtt 
- Install-Package Newtonsoft.Json 
- Install-Package GHIElectronics.UWP.Shields.FEZHAT


##Completed

Intro


````C#

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

        Telemetry temperature = new Telemetry("41c2e437-6c3d-48d0-8e12-81eab2aa5013", "Temperature", "C");
        Telemetry light = new Telemetry("41c2e437-6c3d-48d0-8e12-81eab2aa5014", "Light", "L");

        SecurityManager cm = new SecurityManager("HostName=glovebox-iot-hub.azure-devices.net;DeviceId=RPiSC;SharedAccessKey=z5c+MtYY5zMy7wj3SDiRMpZC7W+UiOkaKTxh/5kP6+c=");

        string hubUser;
        string hubTopicPublish;
        string hubTopicSubscribe;

        private MqttClient client;

        public async void Run(IBackgroundTaskInstance taskInstance) {
            deferral = taskInstance.GetDeferral();

            hubUser = $"{cm.hubAddress}/{cm.hubName}";
            hubTopicPublish = $"devices/{cm.hubName}/messages/events/";
            hubTopicSubscribe = $"devices/{cm.hubName}/messages/devicebound/#";

            this.hat = await FEZHAT.CreateAsync();

            // https://m2mqtt.wordpress.com/m2mqtt_doc/
            client = new MqttClient(cm.hubAddress, 8883, true, MqttSslProtocols.TLSv1_2);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.Subscribe(new string[] { hubTopicSubscribe }, new byte[] { 0 });

            var result = Task.Run(async () => {
                while (true) {
                    if (!client.IsConnected) { client.Connect(cm.hubName, hubUser, cm.hubPass); }
                    if (client.IsConnected) {
                        client.Publish(hubTopicPublish, temperature.ToJson(hat.GetTemperature()));
                        client.Publish(hubTopicPublish, light.ToJson(hat.GetLightLevel()));
                    }
                    await Task.Delay(30000); // don't leave this running for too long at this rate as you'll quickly consume your free daily Iot Hub Message limit
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
 

````
    


>[Home](README.md)