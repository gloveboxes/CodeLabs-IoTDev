>[Home](README.md) </br>
>Previous Lab [Introduction to GPIO aka General Purpose IO](Device-3-GPIO.md)

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

>[Home](README.md) </br>
>Next Lab [Blinky LEDs](Device-5-Blinky.md)