<a name="HOLTop"></a>
# Introduction to Windows 10 IoT Core #

---

<a name="Overview"></a>
## Overview ##

_Did you know Windows 10 runs on the Raspberry Pi?_ In this module, you'll get hands-on experience building a UWP app which runs on tiny devices. Learn how to build, deploy, debug your app, and how to configure the device.

**Windows 10 IoT Core** brings the power of Windows to your device and makes it easy to integrate richer experiences with your devices such as natural user interfaces, searching, online storage and cloud based services

**Windows 10 IoT Core** is a version of **Windows 10** that is optimized for smaller devices with or without a display, and that runs on the **Raspberry Pi, Arrow DragonBoard & MinnowBoard MAX**. **Windows 10 IoT Core** utilizes the rich, extensible [Universal Windows Platform (UWP)](https://msdn.microsoft.com/library/windows/apps/dn726767.aspx) API for building great solutions.


<a name="Objectives"></a>
### Objectives ###
In this module, you'll see how to:

- Connect and configure your device with Windows 10 IoT core
- Create and deploy a "Hello World" UWP app
- Program the application to use the GPIO pins
- Use the FEZ Hat light and temperature sensors

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this module:

- Windows 10 with [developer mode enabled][1]
- [Visual Studio Community 2015][2] with [Update 1][3] or greater
- [Windows IoT Core Project Templates][4]
- [Raspberry PI board with Windows IoT Core image][5]
- [GHI FEZ HAT][6] (for exercises 3 and 4)
- [Windows 10 IoT Core Dashboard][7]
- [Windows IoT Remote Client][8]



> **Note:** The source code for this lab is available on [GitHub](/Module1-IntroWindows10IoTCore/Source).

[1]: https://msdn.microsoft.com/library/windows/apps/xaml/dn706236.aspx
[2]: https://www.visualstudio.com/products/visual-studio-community-vs
[3]: http://go.microsoft.com/fwlink/?LinkID=691134
[4]: https://visualstudiogallery.msdn.microsoft.com/55b357e1-a533-43ad-82a5-a88ac4b01dec
[5]: https://ms-iot.github.io/content/en-US/win10/RPI.htm
[6]: https://www.ghielectronics.com/catalog/product/500
[7]: https://developer.microsoft.com/en-us/windows/iot/getstarted
[8]: https://www.microsoft.com/store/apps/9nblggh5mnxz


<a name="Exercises"></a>
## Exercises ##
This module includes the following exercises:

1. [Connecting and configuring your device](#Exercise1)
1. [Create and deploy "Hello World" UWP](#Exercise2)
1. [Programming the device I/O](#Exercise3)
1. [(Optional) Advanced GPIO](#Exercise4)

Estimated time to complete this module: **60 minutes**


<a name="Exercise1"></a>
### Exercise 1: Connecting and configuring your device ###

The Raspberry Pi will be connected to the development PC through a wired Ethernet connection. 
This connection is used both for deployment and debugging as well as passing through internet requests from the Raspberry Pi when [Internet Connection Sharing](http://ms-iot.github.io/content/en-US/win10/ConnectToDevice.htm) is enabled on the PC.

<a name="Ex1Task1"></a>
#### Task 1 - Identifying the device using IoT Core Dashboard ####

In this task, you'll connect to your device and explore the web management interface.


1. Launch the **Windows 10 IoT Core Dashboard**, go to **My devices** and click the **Open in Device Portal** icon of your device name. 

	![Windows 10 IoT Core Dashboard](Images/ex1task1-watcher.png?raw=true "Windows 10 IoT Core Dashboard")

	_Windows 10 IoT Core Dashboard_

> **Note:** You can also launch the _Device Portal_ by browsing the _IP address_ and adding **:8080**.


1. In the credentials dialog, use the default username and password. Username: _Administrator_ Password: _p@ssw0rd_

	![Device Portal credentials](Images/ex1task1-device-portal-credentials.png?raw=true "Device Portal credentials")

	_Device Portal credentials_

1. **Windows Device Portal** should launch and display the web management home screen!

	![Windows Device Portal](Images/ex1task1-device-portal.png?raw=true "Windows Device Portal")

	_Windows Device Portal_

1. For now, just explore and don't change any settings.

<a name="Ex1Task2"></a>
#### Task 2 - Verify the Windows IoT Remote Client connection  ####

In this task, you will verify the "Windows IoT Core Remote Client" connection.

To start press the Windows key and type “Windows IoT Core Remote Client” and run the app.

It is  likley that you will need to enter the IP address of your Raspberry Pi. Get the address of the device from the **Windows 10 IoT Core Dashboard**.

This will take a moment to connect. When it does you will see the video output of the Raspberry Pi remoted to your desktop.

Minimise the remote client application when you have verified that it is working.

![Windows IoT Remote Client](Images/windows-iot-remote-client.png?raw=true "Windows IoT Remote Client")

<a name="Exercise2"></a>
### Exercise 2: Create and deploy "Hello World" UWP ###

A **Universal Windows Platform (UWP)** app has the potential to run on any Windows-powered device like a Raspberry Pi device with Windows 10 IoT core.

**Windows IoT Core** can be configured for either **headed** or **headless** mode. The difference between these two modes is the presence or absence of any form of UI. By default, Windows 10 IoT Core is in headed mode and runs the default startup app which displays system information like the computer name, connected devices, and IP addresses.

In this exercise, you'll create and deploy a UWP app for headed mode using a XAML view to display a TextBlock and a button that updates the TextBlock content.

<a name="Ex2Task1"></a>
#### Task 1 - Creating a Hello World UWP app ####

In this task, you'll use the Universal project template to create a Blank App. Then you'll add some UI elements and verify the app by debugging the application on your local PC.

1. Start **Visual Studio 2015** and create a new project **File > New Project...**

1. In the installed templates tree, navigate to **Visual C# > Windows > Universal** and select the template **Blank App (Windows Universal)**. Enter the name "IoTWorkshop".

	![New Universal Blank App](Images/ex2task1-new-blank-app.png?raw=true "New Universal Blank App")

	_New Universal Blank App_

1. Click **OK** to create the project.

1. Add a reference to the **Windows IoT extension SDK**. Right-click the **References** entry under the project, select **Add Reference** then navigate the resulting dialog to **Universal Windows > Extensions > Windows IoT Extensions for the UWP**, check the box, and click **OK**.

	![Add Windows IoT extension SDK](Images/ex2task1-IoT-SDK-ref.png?raw=true "Add Windows IoT extension SDK")

	_Add Windows IoT extension SDK_

1. From Solution Explorer, select the **MainPage.xaml** file.

1. Locate the **\<Grid\>** tag within the **XAML** section of the designer, and add the following markup inside the grid. This will add an input and a button that you'll see in the design surface.

	````XML
	<StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
		<TextBox x:Name="Message" Text="Hello, World!" Margin="10" IsReadOnly="True"/>
		<Button x:Name="ClickMe" Content="Click Me!"  Margin="10" HorizontalAlignment="Center"/>
	</StackPanel>
	````

1. Double click the **Click Me!** button in the design surface to generate the click method in the _MainPage.xaml.cs_ file.

1. Add the following line to the **ClickMe_Click** method:

	````C#
	private void ClickMe_Click(object sender, RoutedEventArgs e)
	{
	    this.Message.Text = "Hello, Windows IoT Core!";
	}
	````

1. Press **F5** to build and run the app locally (since this is a Universal Windows Platform (UWP) application, you can test the app on your Visual Studio machine as well).

	![Hello World app running locally](Images/ex2task1-hello-world-local.png?raw=true "Hello World app running locally")

	_Hello World app running locally_

1. Click the **Click Me!** button to verify that the input message changes and close the app after you're done verifying it.

<a name="Ex2Task2"></a>
#### Task 2 - Deploying your app to the device ####

Now that you validated the app in your local PC, you can deploy it to the Raspberry Pi device by using remote debugging.

In order to use remote debugging, your IoT Core device must first be connected to same local network as your development PC.

1. In Visual Studio, continue with the same solution that you created in the previous task and select the **ARM** architecture in the toolbar dropdown.

	![ARM Solution Platform](Images/ex2task2-arm.png?raw=true "ARM Solution Platform")

	_ARM Solution Platform_

1. Next, click the **Device** dropdown and select **Remote Machine**.

	![Run in remote machine](Images/ex2task2-run-remote-machine.png?raw=true "Run in remote machine")

	_Run in remote machine_

1. In  the **Remote Connections** dialog, click your device name within the **Auto Detected** list and then click **Select**. Not all devices can be auto detected, if you don't see it, enter the IP address using the **Manual Configuration**. After entering the device name/IP, select **Universal (Unencrypted Protocol)** Authentication Mode, then click **Select**.

	![Remote Connections dialog](Images/ex2task2-remote-connections-dialog.png?raw=true "Remote Connections dialog")

	_Run in Remote Machine_

    You can verify or modify these values by navigating to the project properties (select Properties in the Solution Explorer) and choosing the **Debug** tab on the left.

1. Build and deploy your app to your device by selecting **Build > Rebuild Solution and Build > Deploy Solution**.

    If there are any missing packages that you did not install during setup, Visual Studio may prompt you to acquire those now.

1. Hit **F5** to deploy and run the app. You can insert breakpoints and debug in the remote device.

1. Restore the "Windows IoT RemoTe Client" app and you should see the output of your app running on the Raspberry Pi.


<a name="Exercise3"></a>
### Exercise 3: Using Windows.Devices.Gpio ###

The **Windows.Devices.Gpio** namespace includes APIs for direct access to the IO pins on the device. Through this API, you can access digital and analog IO, I2C, SPI, and more.

The individual GPIO (General Purpose IO) pins on the Raspberry Pi may be addressed from code using the UWP GPIO APIs. We will use one of these pins to toggle an LED. You will access the GPIO pins using their logical GPIO number, not the the physical pin number. This is consistent with how other APIs and operating systems work on the Raspberry Pi.

![](http://ms-iot.github.io/content/images/PinMappings/RP2_Pinout.png)

<a name="Ex3Task1"></a>
#### Task 1 - Programming the Device IO using the GPIO Controller ####

Of course, you can hook an LED and resistor directly to one of the pins using a breadboard and jumper wires (in fact, we encourage you to try that during the Open Hack), but to keep things simple, we'll toggle the red LED on the GHI FEZ HAT that is already on your Raspberry Pi.

To do this, we'll create a new project.

1. In Visual Studio 2015, create a new **C# Blank App (Universal Windows)**. Name it anything you want. We named ours **IoTHelloBlinky**.

1. When prompted for the SDK version, set the minimum and max SDK version to 10586.

1. You'll use a ToggleButton to turn the LED on and off. In the MainPage.xaml XAML view, place the following markup inside the opening and closing Grid tags.

	````XML
    <ToggleButton x:Name="ToggleLed" Content="Toggle LED" FontSize="40"
                  Padding="15"
                  HorizontalAlignment="Center" VerticalAlignment="Center"
                  Checked="ToggleLed_Checked"
                  Unchecked="ToggleLed_Unchecked" />
    ````

1. Next, you need some code to light up the LED. However, before that, you'll need to add a reference to the IoT UWP extension library to get access to the **Windows.Devices.Gpio** namespace. As before, use the **Project - Add Reference menu** to add the extension. Be sure to check it in the dialog, not just select it. If you have more than one version listed, select the one with the highest number. In a real application, you'll want to keep this in sync with the version of Windows on the device so that you have access to all of the latest features.

![Add IoT Extension SDK](Images/add-iot-extension.png)

1. Now that you have the extension SDK in place, you can add the code. Open the MainPage.xaml.cs code-behind file and add the following namespace to the using statements at the top of the file

	````C#
    using Windows.Devices.Gpio;
    ````

1. Next, add the code to initialize the GPIO controller and pin. The Red LED on the FEZ HAT is connected directly to GPIO 24 on the Raspberry Pi. What this code does is get the default GPIO Controller (which maps to a device driver in Windows), and opens the Red LED Pin for output. This is detailed in the [FEZ HAT schematic](http://www.ghielectronics.com/downloads/schematic/FEZ_HAT_SCH.pdf). Finally, it writes a **low** value to the pin to turn the LED off.

	````C#
    public MainPage()
    {
        this.InitializeComponent();

        InitializeGpio();
    }

    private const int LED_PIN = 24;
    private GpioPin _pin;
    private void InitializeGpio()
    {
        var controller = GpioController.GetDefault();

        if (controller != null)
        {
            _pin = controller.OpenPin(LED_PIN);

            _pin.SetDriveMode(GpioPinDriveMode.Output);

            _pin.Write(GpioPinValue.Low);
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Target device has no GPIO controller");
        }
    }
    ````

1. At this point, we have the pin opened and set to the right mode. The final step is to actually toggle the pin's state when the toggle button is pressed. Add the following event handler code to the same code-behind file.

	````C#
    private void ToggleLed_Checked(object sender, RoutedEventArgs e)
    {
        if (_pin != null)
            _pin.Write(GpioPinValue.High);
    }

    private void ToggleLed_Unchecked(object sender, RoutedEventArgs e)
    {
        if (_pin != null)
            _pin.Write(GpioPinValue.Low);
    }
    ````

1. The code is complete. Now follow the same deployment steps you used in the previous exercise to deploy the app to the Raspberry Pi. (First, set the target to **ARM**, then select the **Remote Machine**).

1. Again from the "Windows IoT Remote Client" and using the mouse, click the button on and off and look at the red LED on the board. If you want to see the debugging in action, place a breakpoint in both of the event handlers and step through when you click the button.

<a name="Exercise4"></a>
### Exercise 4: Programming the device I/O using the FEZ HAT###

The [GHI FEZ HAT](https://www.ghielectronics.com/catalog/product/500) allows for a Fast and Easy (FEZ) way to connect all kinds of sensors and devices to the Raspberry Pi. This topping includes several features like temperature and light sensors, digital and analog I/O, buttons, motor connectors, among others.

In this exercise, you'll again use the red LED, but using the GHI driver code, and turning it on and off with a timer rather than a button. In the following exercise you'll use the temperature and light sensors and the 2 RGB LEDs.

<a name="Ex4Task1"></a>
#### Task 1 - GPIO and lighting up an LED ####

In this task, you'll add the **FEZ HAT driver** (using the NuGet package) so you can control the FEZ HAT's LED and make it blink, using the driver library. There are some differences with the code to light up the FEZ HAT LEDs. If you want to explore the "raw" GPIO approach, we have examples in the Open Hack lab. For our follow-up Azure work, it's important to familiarize with the HAT APIs. This is akin to using Arduino shields and their libraries vs. raw IO.

1. Open the **IoTWorkshop.sln** solution file from the **Begin** folder of this exercise, or you can continue working with your solution from the previous exercise.

1. Install the FEZ HAT drivers using the [GHIElectronics.UWP.Shields.FEZHAT NuGet package](https://www.nuget.org/packages/GHIElectronics.UWP.Shields.FEZHAT/ "FEZ HAT NuGet Package"). To do this, open the **Package Manager Console** (_Tools > NuGet Package Manager > Package Manager Console_) and execute the following command: (You could instead add the package using the GUI if you desire.)

	````PowerShell
	PM> Install-Package GHIElectronics.UWP.Shields.FEZHAT
	````

	![Installing GHI Electronics NuGet package](Images/ex3task1-intalling-ghi-electronics-nuget-package.png?raw=true)

	_Installing the FEZ hat Nuget package_

1. Next, add a reference to the FEZ HAT library namespace in the _MainPage.xaml.cs_ file:

	````C#
	using GHIElectronics.UWP.Shields;
	````

1. At the class level, define the variables that will hold the reference to the following objects:
  - **hat**: of type **Shields.FEZHAT**, will contain the hat driver object that you'll use to communicate with the FEZ hat through the Raspberry.
  - **timer**: of type **DispatchTimer**, that will be used to turn the led at regular basis.
  - **next**: of type **bool**, will hold the next on/off status for the led.

	````C#
	private FEZHAT hat;
	private DispatcherTimer timer;
	private bool next;
	````

1. Add the following method to initialize the objects used to handle the communication with the hat. The **Timer_Tick** method will be defined next, and will be executed every 500 ms according to the value hardcoded in the **Interval** property.

	````C#
	private async void SetupHat()
	{
	    this.hat = await FEZHAT.CreateAsync();

	    this.timer = new DispatcherTimer();

	    this.timer.Interval = TimeSpan.FromMilliseconds(500);
	    this.timer.Tick += this.Timer_Tick;

	    this.timer.Start();
	}
	````

1. The following method will be executed every time the timer ticks, and will turn the LED on/off.

	````C#
	private void Timer_Tick(object sender, object e)
	{
	    this.hat.DIO24On = this.next;
	    System.Diagnostics.Debug.WriteLine("LED turned " + (this.next ? "on" : "off"));
	    this.next = !this.next;
	}
	````

	The first statement sets the LED to the next on/off status, the second line shows the new status, and the last line switches the next value.

1. Before running the application, add the call to the **SetupHat** method in the **MainPage** constructor:

	<!-- mark:5-6 -->
	````C#
	public MainPage()
	{
	    this.InitializeComponent();

	    // Initialize FEZ HAT shield
	    this.SetupHat();	    
	}
	````

1. Now you're ready to run the application. Make sure the Raspberry Pi is connected with the FEZ HAT and deploy the application (find the steps to deploy in the previous exercise).

	![Running application](Images/ex3task1-raspberry-led.png?raw=true "Running application")

	_Running application_

<a name="Ex4Task2"></a>
#### Task 2 - Blinking the LED based on a button press ####

In this task, you'll refactor the application to use the button from the FEZ HAT to make the LED blink.

1. In the **Timer_Tick** method, add the following code to read when the buttons are pressed.

	````C#
	var btn1 = this.hat.IsDIO18Pressed();
	var btn2 = this.hat.IsDIO22Pressed();
	````

1. Enclose the code to make the LED blink in an **if** statement to be executed whenever any of the buttons are pressed.

	<!-- mark:6-16 -->
	````C#
	private void Timer_Tick(object sender, object e)
	{
		var btn1 = this.hat.IsDIO18Pressed();
		var btn2 = this.hat.IsDIO22Pressed();

		if (btn1 || btn2)
		{
			this.hat.DIO24On = this.next;
			System.Diagnostics.Debug.WriteLine("LED turned " + (this.next ? "on" : "off"));
			this.next = !this.next;
		}
		else
		{
			this.hat.DIO24On = false;
		}
	}
	````

1. Hit **F5** to re-deploy the application to the device. The LED should blink only while you press and hold any of the buttons.

	![LED lightning while pressing the button](Images/ex3task2-raspberry-led-button.png?raw=true "LED lightning while pressing the button")

	_LED lightning while pressing the button_

<a name="Exercise5"></a>
### (Optional) Exercise 5: Advanced GPIO ###

In addition to the red LED and buttons in the FEZ HAT, there are also temperature and light sensors.

In this exercise, you'll refactor the UI to display the room temperature and light level values from the FEZ HAT sensors.

<a name="Ex5Task1"></a>
#### Task 1 - Getting light and temperature information ####

The FEZ HAT driver includes methods to get the temperature and light levels. In this task, you'll refactor the UI to display those values.

1. Open the **IoTWorkshop.sln** solution file from the **Begin** folder of this exercise or you can continue working with your solution from the previous exercise.

1. In the XAML code (**MainPage.xaml** file), replace the content of the **StackPanel** with the following markup.

	````XML
	<StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
		 <StackPanel Orientation="Horizontal">
			  <TextBlock Text="Light:"></TextBlock>
			  <ProgressBar x:Name="LightProgress" Value="0" Minimum="0" Maximum="1" Width="150"></ProgressBar>
			  <TextBlock x:Name="LightTextBox"></TextBlock>
		 </StackPanel>
		 <StackPanel Orientation="Horizontal">
			  <TextBlock Text="Temp:"></TextBlock>
			  <ProgressBar x:Name="TempProgress" Value="0" Minimum="0" Maximum="60" Width="150"></ProgressBar>
			  <TextBlock x:Name="TempTextBox"></TextBlock>
		 </StackPanel>
	</StackPanel>
	````

    This will include TextBlocks and StackPanels to show the temperature and light levels.

1. Add code in the **Timer_Tick** method to read the light level and temperature from the FEZ HAT sensors.

	<!-- mark:5-9 -->
	````C#
	private void Timer_Tick(object sender, object e)
	{
		 //...

		 // Light Sensor
		 var light = this.hat.GetLightLevel();

		 // Temperature Sensor
		 var temp = this.hat.GetTemperature();
	}
	````

1. Display the sensor values in the UI controls.

	<!-- mark:5-11 -->
	````C#
	private void Timer_Tick(object sender, object e)
	{
	    //...

	    // Display values
	    this.LightTextBox.Text = light.ToString("P2");
	    this.LightProgress.Value = light;
	    this.TempTextBox.Text = temp.ToString("N2");
	    this.TempProgress.Value = temp;

	    System.Diagnostics.Debug.WriteLine("Temperature: {0} °C, Light {1}", temp.ToString("N2"), light.ToString("N2"));
	}
	````

1. Hit **F5** to re-deploy the application to the device. 

1. Restore the "Windows IoT Remote Client" and you should see the values shown in the display. You can see how the light percentage varies by approaching your hand to the light sensor.

	> **Note:** You should set up the device deployment if you didn't continue working with the previous solution.

	![Running application](Images/ex4task1-running-application.png?raw=true)

	_Running application_

<a name="Ex5Task2"></a>
#### Task 2 - Using the RGB LED  ####

In this task, you'll use the 2 RGB LEDs in the FEZ HAT to output light with different intensities, according to the temperature and light in the room.


1. In the **Timer_Tick** method, add the following code to calculate a value from 0 to 255 depending on the light and temperature intensities.

	````C#
	var lightIntensity = (byte)(light * 255);
	var tempIntensity = (byte)(temp * 255 / 60);
	````
    We need a 0-255 range value for one of the RGB components of color.

1. Set the color of the RGB values using the light and temperature intensities. Use the Red component for the temperature, and Blue for light level.

	````C#
	this.hat.D2.Color = new FEZHAT.Color(tempIntensity, 0, 0);
	this.hat.D3.Color = new FEZHAT.Color(0, 0, lightIntensity);
	````

    With this code we are lighting the first RGB LED in red color for the temperature. The more it shines, the more heat the sensor is reading. For the second RGB LED, we use the blue color to output the amount of light.

1. Re-deploy the application to the device. Try changing the temperature and light levels by approaching your hand to the device and notice the RGB LEDs variations.

	![Running application](Images/ex4task2-raspberry-light-temp.png?raw=true "Running application")

	_Running application_

---

<a name="Summary"></a>
## Summary ##

By completing this module, you should have:

- Learned how to connect and configure a **Raspberry Pi** device with **Windows 10 IoT Core**
- Created a **Windows Universal Platform** app and deployed to a remote Windows IoT device
- Toggled an on-board red LED using standard Windows.Devices.Gpio APIs
- Programmed the input (buttons) and output (LEDs) of a GHI's FEZ HAT topping connected to the Raspberry Pi
- Read the temperature and light sensors and set the RGB LEDs colors from the FEZ HAT


