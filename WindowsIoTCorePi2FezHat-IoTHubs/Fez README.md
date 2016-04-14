Windows 10 IoT Core Hands-on Lab
========================================


### Setting up your Azure Account


For this lab you will need access to an Azure subscription

If you don’t already have an Azure account, then you will need to provision one.

There are currently two free trail offers – either good for the purposes of the workshop.

1. [Visual Studio Dev Essentials](https://www.visualstudio.com/en-us/products/visual-studio-dev-essentials-vs.aspx) Sign up for free. $25 a month for a year. More slow and steady over an extended period of time.
2. [Free one-month trial](https://azure.microsoft.com/en-us/pricing/free-trial/). Sign up for free and get $200 to spend on all Azure services. Great is you really want to exercise lots of Azure capabilities for a limited period of time.

Valid credit card information is required for identity verification purposes only. Your credit card will not be charged for this offer unless you explicitly remove the spending limit.


###Exercises 

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this module:


- Windows 10 with [developer mode enabled][1]
- [Visual Studio Community 2015][2] with [Update 1][3] or greater
- [Windows IoT Core Project Templates][4]
- [Raspberry PI board with Windows IoT Core image][5]
- [GHI FEZ HAT][6]
- [Windows 10 IoT Core Dashboard][7]
- [Windows IoT Remote Client][8] see [Remote Display Experience](https://developer.microsoft.com/en-us/windows/iot/win10/remotedisplay)
- [IoT Hub Device Explorer][9] (Scroll down for SetupDeviceExplorer.msi)
- [An Azure Account][10]



> **Note:** The source code for this lab is available on [GitHub](/Module1-IntroWindows10IoTCore/Source).

[1]: https://msdn.microsoft.com/library/windows/apps/xaml/dn706236.aspx
[2]: https://www.visualstudio.com/products/visual-studio-community-vs
[3]: http://go.microsoft.com/fwlink/?LinkID=691134
[4]: https://visualstudiogallery.msdn.microsoft.com/55b357e1-a533-43ad-82a5-a88ac4b01dec
[5]: https://ms-iot.github.io/content/en-US/win10/RPI.htm
[6]: https://www.ghielectronics.com/catalog/product/500
[7]: https://developer.microsoft.com/en-us/windows/iot/getstarted
[8]: https://www.microsoft.com/store/apps/9nblggh5mnxz
[9]: https://github.com/Azure/azure-iot-sdks/releases
[10]: AzureAccount.md


<a name="Exercises"></a>
## Exercises ##
This module includes the following exercises:

1. [Connecting and configuring your device](#Exercise1)
1. [Setting up your Azure Account](#Task13)
1. [Creating an IoT Hub](#CreatingIoTHub)
1. [Creating a Stream Analitycs Job](#CreatingStreamAnaltics)
1. [Registering your device](#RegisterDevice)
1. [Sending telemetry data to the Azure IoT hub](SendingTelemetry)
1. [Consuming the IoT Hub data](#ConsumingData)

Estimated time to complete this module: **60 minutes**












<a name="Summary" />
## Summary
In this lab, you have learned how to create a Universal app that reads from the sensors of a FEZ hat connected to a Raspberry Pi 2 running Windows 10 IoT Core, and upload those readings to an Azure IoT Hub. You also learned how to read and consume the information in the IoT Hub using Power BI to get near real-time analysis of the information gathered from the FEZ hat sensors and to create simple reports and how to consume it using a website. You also saw how to use the IoT Hubs Cloud-To-Device messages feature to send simple commands to your devices.
