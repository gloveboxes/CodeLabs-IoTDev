>[Home](README.md) </br>
>Previous Lab [Programming IO](Device-4-Programming-IO.md)

#### Exercise 5: - Blinking the LED based on a button press ####

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
	
>[Home](README.md) </br>
>Next Lab [Advanced GPIO](Device-6-Advanced-GPIO.md)