Windows 10 IoT Core Hands-on Lab
========================================

<a name="Task13" />
### Setting up your Azure Account


For this lab you will need access to an Azure subscription

If you don’t already have an Azure account, then you will need to provision one.

There are currently two free trail offers – either good for the purposes of the workshop.

1. [Visual Studio Dev Essentials](https://www.visualstudio.com/en-us/products/visual-studio-dev-essentials-vs.aspx) Sign up for free. $25 a month for a year. More slow and steady over an extended period of time.
2. [Free one-month trial](https://azure.microsoft.com/en-us/pricing/free-trial/). Sign up for free and get $200 to spend on all Azure services. Great is you really want to exercise lots of Azure capabilities for a limited period of time.

Valid credit card information is required for identity verification purposes only. Your credit card will not be charged for this offer unless you explicitly remove the spending limit.




#### Creating an IoT Hub

1. Enter the Azure portal, by browsing to http://portal.azure.com
2. Create a new IoT Hub. To do this, click **New** in the jumpbar, then click **Internet of Things**, then click **Azure IoT Hub**.

	![Creating a new IoT Hub](Images/creating-a-new-iot-hub.png?raw=true "Createing a new IoT Hub")

	_Creating a new IoT Hub_

3. Configure the **IoT hub** with the desired information:
 - Enter a **Name** for the hub e.g. _iot-sample_,
 - Select a **Pricing and scale tier** (_F1 Free_ tier is enough),
 - Create a new resource group, or select and existing one. For more information, see [Using resource groups to manage your Azure resources](https://azure.microsoft.com/en-us/documentation/articles/resource-group-portal/).
 - Select the **Region** such as _East US_ where the service will be located.

	![new iot hub settings](Images/new-iot-hub-settings.png?raw=true "New IoT Hub settings")

	_New IoT Hub Settings_

4. It can take a few minutes for the IoT hub to be created. Once it is ready, open the blade of the new IoT hub, take note of the URI and select the key icon at the top to access to the shared access policy settings:

	![IoT hub shared access policies](Images/iot-hub-shared-access-policies.png?raw=true)

5. Select the Shared access policy called **iothubowner**, and take note of the **Primary key** and **connection string** in the right blade.  You should copy these into a text file for future use. 

	![Get IoT Hub owner connection string](Images/get-iot-hub-owner-connection-string.png?raw=true)

#### Creating a Stream Analitycs Job

To create a Stream Analytics Job, perform the following steps.

1. Currently the new Azure Portal doesn't support all of the features of Stream Analytics required for this lab.  For that reason, the Stream Analaytics configuration needs to be done in the **classic portal**.   To open the classic portal, in your browser navigate to https://manage.windowsazure.com and login in with your Azure Subscription's credentials. 

1. In the Azure Management Portal, click **NEW**, then click **DATA SERVICES**, then **STREAM ANALYTICS**, and finally **QUICK CREATE**.

2. Enter the **Job Name**, select a **Region**, such as _East US_ (if it is an option, create the Stream Analytics Job in the same region as your IoT Hub) and the enter a **NEW STORAGE ACCOUNT NAME** if this is the first storage account in the region used for Stream Analitycs, if not you have to select the one already used for that region.

3. Click **CREATE A STREAM ANALITYCS JOB**.

	![Creating a Stream Analytics Job](Images/createStreamAnalytics.png?raw=true)

	_Creating a Stream Analytics Job_

4. After the _Stream Analytics_ job is created, in the left pane click **STORAGE**, then click the account you used in the previous step, and click **MANAGE ACCESS KEYS**. As with the IoT Hub details, copy the **STORAGE ACCOUNT NAME** and the **PRIMARY ACCESS KEY** into a text file as you will use those values later.

	![manage access keys](Images/manage-access-keys.png?raw=true)

	_Managing Access Keys_

<a name="Task14" />
### Registering your device
You must register your device in order to be able to send and receive information from the Azure IoT Hub. This is done by registering a [Device Identity](https://azure.microsoft.com/en-us/documentation/articles/iot-hub-devguide/#device-identity-registry) in the IoT Hub.

1. Open the Device Explorer app (C:\Program Files (x86)\Microsoft\DeviceExplorer\DeviceExplorer.exe) and fill the **IoT Hub Connection String** field with the connection string of the IoT Hub you created in previous steps and click on **Update**.

	![Configure Device Explorer](Images/configure-device-explorer.png?raw=true)

2. Go to the **Management** tab and click on the **Create** button. The Create Device popup will be displayed. Fill the **Device ID** field with a new Id for your device (_myFirstDevice_ for example) and click on Create:

	![Creating a Device Identity](Images/creating-a-device-identity.png?raw=true)

3. Once the device identity is created, it will be displayed in the grid. Right click on the identity you just created, select **Copy connection string for selected device** and take note of the value copied to your clipboard, since it will be required to connect your device with the IoT Hub.

	![Copying Device connection information](Images/copying-device-connection-information.png?raw=true)

	> **Note:** The device identities registration can be automated using the Azure IoT Hubs SDK. An example of how to do that can be found [here](https://azure.microsoft.com/en-us/documentation/articles/iot-hub-csharp-csharp-getstarted/#create-a-device-identity).



<a name="Ex2Task3"></a>
#### Task 3 - Sending telemetry data to the Azure IoT hub ####


In order to get the information out of the hat sensors, you will take advantage of the Developers' Guide that GHI Electronics published.

Now that the device is configured, you'll see how to make an application read the values of the FEZ HAT sensors, and then send those values to an Azure IoT Hub.

This task uses an existing Universal application that will be deployed to your Raspberry Pi device and use FEZ HAT sensors.

1. Open in Visual Studio the **IoTWorkshop.sln** solution located at **Source\Ex2\Begin** folder.

1. In **Solution Explorer**, right-click the **IoTWorkshop** project, and then click **Manage NuGet Packages**.

1. In the **NuGet Package Manager** window, click **Browse** and search for **Microsoft Azure Devices** and **PCL Crypto**, click **Install** to install the **Microsoft.Azure.Devices.Client** and **PCLCrypto** packages, and accept the terms of use.

    This downloads, installs, and adds a reference to the [Microsoft Azure IoT Service](https://www.nuget.org/packages/Microsoft.Azure.Devices/) SDK NuGet package.

1. Add the following _using_ statements at the top of the **MainPage.xaml.cs** file:

	````C#
	using Microsoft.Azure.Devices.Client;
	````

1. Add the following field to the **MainPage** class, replace the placeholder value with the **device connection string** you've created in the previous task (note that the curly braces { } are _NOT_ part of the connection string and should be _removed_ when you paste in your connection string):

	````C#
	private DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("{device connection string}");
	````

1. Add the following method to the **MainPage** class to create and send messages to the IoT hub. Resolve the missing using statements.

	````C#
	public async void SendMessage(string message)
	{
		 // Send message to an IoT Hub using IoT Hub SDK
		 try
		 {
			  var content = new Message(Encoding.UTF8.GetBytes(message));
			  await deviceClient.SendEventAsync(content);

			  Debug.WriteLine("Message Sent: {0}", message, null);
		 }
		 catch (Exception e)
		 {
			  Debug.WriteLine("Exception when sending message:" + e.Message);
		 }
	}
	````

1. Add the following code to the **Timer_Tick** method to send a message with the temperature and another with the light level:

	````C#
	// send data to IoT Hub
	var jsonMessage = string.Format("{{ displayname:null, location:\"USA\", organization:\"Fabrikam\", guid: \"41c2e437-6c3d-48d0-8e12-81eab2aa5013\", timecreated: \"{0}\", measurename: \"Temperature\", unitofmeasure: \"C\", value:{1}}}",
		 DateTime.UtcNow.ToString("o"),
		 temp);

	this.SendMessage(jsonMessage);

	jsonMessage = string.Format("{{ displayname:null, location:\"USA\", organization:\"Fabrikam\", guid: \"41c2e437-6c3d-48d0-8e12-81eab2aa5013\", timecreated: \"{0}\", measurename: \"Light\", unitofmeasure: \"L\", value:{1}}}",
		 DateTime.UtcNow.ToString("o"),
		 light);

	this.SendMessage(jsonMessage);
	````

	1. In order to deploy your app to the IoT device, select the **ARM** architecture in the Solution Platforms dropdown.

		![ARM Solution Platform](Images/arm-platform.png?raw=true "ARM Solution Platform")

		_ARM Solution Platform_

	1. Next, click the **Device** dropdown and select **Remote Machine**.

		![Run in remote machine](Images/run-remote-machine.png?raw=true "Run in remote machine")

		_Run in remote machine_

	1. In  the **Remote Connections** dialog, click your device name within the **Auto Detected** list and then click **Select**. Not all devices can be auto detected, if you don't see it, enter the IP address using the **Manual Configuration**. After entering the device name/IP, select **Universal (Unencrypted Protocol)** Authentication Mode, then click **Select**.

		![Remote Connections dialog](Images/remote-connections-dialog.png?raw=true "Remote Connections dialog")

		_Remote Connections dialog_

1. Press **F5** to run and deploy the app to the device.

	The information being sent can be monitored using the Device Explorer application. Run the application and go to the **Data** tab and select the name of the device you want to monitor (_myRaspberryDevice_ in your case), then click  **Monitor**.

	![Monitoring messages sent](Images/monitoring-messages-sent.png?raw=true "Monitoring messages sent")

	_Monitoring messages sent_

    **Note**: If you navigate back to your IoT Hub blade in the Azure Portal, it may take a couple minutes before the message count is updated to reflect the device activity under **Usage**.

<a name="Task3" />
## Consuming the IoT Hub data
You have seen how to use the Device Explorer to peek the data being sent to the Azure IoT Hub. However, the Azure IoT suite offers many different ways to generate meaningful information from the data gathered by the devices. In the following sections you will explore two of them: You will see how the Azure Services Bus Messaging system can be used in a Website (part of the ConnectTheDots project), and how to use Azure Stream Analytics in combination with Microsoft Power BI to consume the data and to generate meaningful reports.

<a name="Task32" />
### Using Power BI

One of the most interesting ways to use the information received from the connected device/s is to get near real-time analysis using the **Microsoft Power BI** tool. In this section you will see how to configure this tool to get an online dashboard showing summarized information about the different sensors.

<a name="Task321" />
#### Setting up a Power BI account
If you don't have a Power BI account already, you will need to create one (a free account is enough to complete this lab). If you already have an account set you can skip this step.

1. Go to the [Power BI website](https://powerbi.microsoft.com/) and follow the sign-up process.

	> **Note:** At the moment this lab was written, only users with corporate email accounts are allowed to sign up. Free consumer email accounts (like Outlook, Hotmail, Gmail, Yahoo, etc.) can't be used.

2. You will be asked to enter your email address. Then a confirmation email will be sent. After following the confirmation link, a form to enter your personal information will be displayed. Complete the form and click Start.

	The preparation of your account will take several minutes, and when it's ready you will see a screen similar to the following:

	![Power BI Welcome screen](Images/power-bi-welcome-screen.png?raw=true)

	_Power BI welcome screen_

Now that your account is set, you are ready to set up the data source that will feed the Power BI dashboard.

<a name="Task3220" />
##### Create a Service Bus Consumer Group
In order to allow several consumer applications to read data from the IoT Hub independently at their own pace a Consumer Group must be configured for each one. If all of the consumer applications (the Device Explorer, Stream Analytics / Power BI, the Web site you will configure in the next section) read the data from the default consumer group, one application will block the others.

To create a new Consumer Group for the IoT Hub that will be used by the Stream Analytics job you are about to configure, follow these steps:

- Open the Azure Portal (https://portal.azure.com/), and select the IoT Hub you created.
- From the settings blade, click on **Messaging**
- At the bottom of the Messaging blade, type the name of the new Consumer Group "PowerBI"
- From the top menu, click on the Save icon

![Create Consumer Group](Images/create-consumer-group.png?raw=true)

<a name="Task322" />
#### Setting the data source
In order to feed the Power BI reports with the information gathered by the hats and to get that information in near real-time, **Power BI** supports **Azure Stream Analytics** outputs as data source. The following section will show how to configure the Stream Analytics job created in the Setup section to take the input from the IoT Hub and push that summarized information to Power BI.

<a name="Task3221" />
##### Stream Analytics Input Setup
Before the information can be delivered to **Power BI**, it must be processed by a **Stream Analytics Job**. To do so, an input for that job must be provided. As the Raspberry devices are sending information to an IoT Hub, it will be set as the input for the job.

1. Go to the classic [Azure management portal](Stream Analytics Input ) (https://manage.windowsazure.com) and select the **Stream Analytics** service. There you will find the Stream Analytics job created during the _Azure services setup_. Click on the job to enter the Stream Analytics configuration screen.

	![Stream Analytics configuration](Images/stream-analytics-configuration.png?raw=true)

	_Stream Analytics Configuration_

2. As you can see, the Start button is disabled since the job is not configured yet. To set the job input click on the **INPUTS** tab and then in the **Add an input** button.

3. In the **Add an input to your job** popup, select the **Data Stream** option and click **Next**. In the following step, select the option **IoT Hub** and click **Next**. Lastly, in the **IoT Hub Settings** screen, provide the following information:

	- **Input Alias:** _TelemetryHub_
	- **Subscription:** Use IoT Hub from Current Subscription (you can use an Event Hub from another subscription too by selecting the other option)
	- **Choose an IoT Hub:** _iot-sample_ (or the name used during the IoT Hub creation)
	- **IoT Hub Shared Access Policy Name:** _iothubowner_
	- **IoT Hub Consumer Group:** _powerbi_

	![Stream Analytics Input configuration](Images/stream-analytics-input-configuration.png?raw=true)

	_Stream Analytics Input Configuration_

4. Click **Next**, and then **Complete** (leave the Serialization settings as they are).

<a name="Task3222" />
##### Stream Analytics Output Setup
The output of the Stream Analytics job will be Power BI.

1. To set up the output, go to the Stream Analytics Job's **OUTPUTS** tab, and click the **ADD AN OUTPUT** link.

2. In the **Add an output to your job** popup, select the **POWER BI** option and the click the **Next button**.

3. In the following screen you will setup the credentials of your Power BI account in order to allow the job to connect and send data to it. Click the **Authorize Now** link.

	![Stream Analytics Output configuration](Images/steam-analytics-output-configuration.png?raw=true)

	_Stream Analytics Output Configuration_

	You will be redirected to the Microsoft login page.

4. Enter your Power BI account email and password and click **Continue**. If the authorization is successful, you will be redirected back to the **Microsoft Power BI Settings** screen.

5. In this screen you will enter the following information:

	- **Output Alias**: _PowerBI_
	- **Dataset Name**: _Raspberry_
	- **Table Name**: _Telemetry_
	- **Group Name**: _My Workspace_

	![Power BI Settings](Images/power-bi-settings.png?raw=true)

	_Power BI Settings_

6. Click the checkmark button to create the output.

<a name="Task3223" />
##### Stream Analytics Query configuration
Now that the job's inputs and outputs are already configured, the Stream Analytics Job needs to know how to transform the input data into the output data source. To do so, you will create a new Query.

1. Go to the Stream Analytics Job **QUERY** tab and replace the query with the following statement:

	````SQL
	SELECT
		iothub.iothub.connectiondeviceid displayname,
		location,
		guid,
		measurename,
		unitofmeasure,
		Max(timecreated) timecreated,
		Avg(value) AvgValue
	INTO
		[PowerBI]
	FROM
		[TelemetryHUB] TIMESTAMP by timecreated
	GROUP BY
		iothub.iothub.connectiondeviceid, location, guid, measurename, unitofmeasure,
		TumblingWindow(Second, 10)
	````

	The query takes the data from the input (using the alias defined when the input was created **TelemetryHUB**) and inserts into the output (**PowerBI**, the alias of the output) after grouping it using 10 seconds chunks.

2. Click on the **SAVE** button and **YES** in the confirmation dialog.

<a name="Task3234" />
##### Starting the Stream Analytics Job
Now that the job is configured, the **START** button is enabled. Click the button to start the job and then select the **JOB START TIME** option in the **START OUTPUT** popup. After clicking **OK** the job will be started.

Once the job starts it creates the Power BI datasource associated with the given subscription.

<a name="Task324" />
#### Setting up the Power BI dashboard
1. Now that the datasource is created, go back to your Power BI session, and go to **My Workspace** by clicking the **Power BI** link.

	After some minutes of the job running you will see that the dataset that you configured as an output for the Job, is now displayed in the Power BI workspace Datasets section.

	![Power BI new datasource](Images/power-bi-new-datasource.png?raw=true)

	_Power BI: New Datasource_

	> **Note:** The Power BI dataset will only be created if the job is running and if it is receiving data from the IoT Hub input, so check that the Universal App is running and sending data to Azure to ensure that the dataset be created. To check if the Stream Analytics job is receiving and processing data you can check the Azure management Stream Analytics monitor.

2. Once the datasource becomes available you can start creating reports. To create a new Report click on the **Raspberry** datasource:

	![Power BI Report Designer](Images/power-bi-report-designer.png?raw=true)

	_Power BI: Report Designer_

	The Report designer will be opened showing the list of fields available for the selected datasource and the different visualizations supported by the tool.

3. To create the _Average Light by time_ report, select the following fields:

	- avgvalue
	- timecreated

	As you can see the **avgvalue** field is automatically set to the **Value** field and the **timecreated** is inserted as an axis. Now change the chart type to a **Line Chart**:

	![Select Line Chart](Images/select-line-chart.png?raw=true)

	_Selecting the Line Chart_

4. Then you will set a filter to show only the Light sensor data. To do so drag the **measurename** field to the **Filters** section and then select the **Light** value:

	![Select Report Filter](Images/select-report-filter.png?raw=true)
	![Select Light sensor values](Images/select-light-sensor-values.png?raw=true)

	_Selecting the Report Filters_

5. Now the report is almost ready. Click the **SAVE** button and set _Light by Time_ as the name for the report.

	![Light by Time Report](Images/light-by-time-report.png?raw=true)

	_Light by Time Report_

6. Now you will create a new Dashboard, and pin this report to it. Click the plus sign (+) next to the **Dashboards** section to create a new dashboard. Set _Raspberry Telemetry_ as the **Title** and press Enter. Now, go back to your report and click the pin icon to add the report to the recently created dashboard.

	![Pin a Report to the Dashboard](Images/pin-a-report-to-the-dashboard.png?raw=true)

	_Pinning a Report to the Dashboard_

1. To create a second chart with the information of the average Temperature follow these steps:
	1. Click on the **Raspberry** datasource to create a new report.
	2. Select the **avgvalue** field
	3. Drag the **measurename** field to the filters section and select **Temperature**
	4. Now change the visualization to a **gauge** chart:

		![Change Visualization to Gauge](Images/change-visualization-to-gauge.png?raw=true "Gauge visualization")

		_Gauge visualization_

	5. Change the **Value** from **Sum** to **Average**

		![Change Value to Average](Images/change-value-to-average.png?raw=true)

		_Change Value to Average_

		Now the Report is ready:

		![Gauge Report](Images/gauge-report.png?raw=true)

		_Gauge Report_

	6. Save and then Pin it to the Dashboard.

7. Following the same directions, create a _Temperature_ report and add it to the dashboard.
8. Lastly, edit the reports name in the dashboard by clicking the pencil icon next to each report.

	![Edit Report Title](Images/edit-report-title.png?raw=true)

	_Editing the Report Title_

	After renaming both reports you will get a dashboard similar to the one in the following screenshot, which will be automatically refreshed as new data arrives.

	![Final Power BI Dashboard](Images/final-power-bi-dashboard.png?raw=true)

	_Final Power BI Dashboard_


<a name="Task4">
## Sending commands to your devices
Azure IoT Hub is a service that enables reliable and secure bi-directional communications between millions of IoT devices and an application back end. In this section you will see how to send cloud-to-device messages to your device to command it to change the color of one of the FEZ HAT leds, using the Device Explorer app as the back end.

1. Open the Universal app you created before and add the following method to the **ConnectTheDotsHelper.cs** file:

	````C#
	public async Task<string> ReceiveMessage()
	{
		if (this.HubConnectionInitialized)
		{
			try
			{
				var receivedMessage = await this.deviceClient.ReceiveAsync();

				if (receivedMessage != null)
				{
					var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
					this.deviceClient.CompleteAsync(receivedMessage);
					return messageData;
				}
				else
				{
					return string.Empty;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception when receiving message:" + e.Message);
				return string.Empty;
			}
		}
		else
		{
			return string.Empty;
		}
	}
	````

	The _ReceiveAsync_ method returns the received message at the time that it is received by the device. The call to _CompleteAsync()_ notifies IoT Hub that the message has been successfully processed and that it can be safely removed from the device queue. If something happened that prevented the device app from completing the processing of the message, IoT Hub will deliver it again.
	
2. Now you will add the logic to process the messages received. Open the **MainPage.xaml.cs** file and add a new timer to the _MainPage_ class:

	````C#
	DispatcherTimer commandsTimer;
	````

3. Add the following method, which will be on charge of processing the commands:

	````C#
	private async void CommandsTimer_Tick(object sender, object e)
	{
		string message = await ctdHelper.ReceiveMessage();

		if (message != string.Empty)
		{
			System.Diagnostics.Debug.WriteLine("Command Received: {0}", message);
			switch (message.ToUpperInvariant())
			{
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
	````

	It reads the message received, and according to the text of the command, it set the value of the _hat.D2.Color_ attribute to change the color of the FEZ HAT's LED D2. When the "OFF" command is received the _TurnOff()_ method is called, which turns the LED off.
	
4. Lastly, add the following piece of code to the _SetupHatAsync_ method in order to initialize the timer used to poll for messages. 

	````C#
	this.commandsTimer = new DispatcherTimer();
	this.commandsTimer.Interval = TimeSpan.FromSeconds(60);
	this.commandsTimer.Tick += this.CommandsTimer_Tick;
	this.commandsTimer.Start();
	````

	> **Note:** The recommended interval for HTTP/1 message polling is 25 minutes. For debugging and demostration purposes a 1 minute polling interval is fine (you can use an even smaller interval for testing), but bear it in mind for production development. Check this [article](https://azure.microsoft.com/en-us/documentation/articles/iot-hub-devguide/) for guidance. When AMQP becomes available for the IoT Hub SDK using UWP apps a different approach can be taken for message processing, since AMQP supports server push when receiving cloud-to-device messages, and it enables immediate pushes of messages from IoT Hub to the device. The following [article](https://azure.microsoft.com/en-us/documentation/articles/iot-hub-csharp-csharp-c2d/) explains how to handle cloud-to-device messages using AMQP.
	
5. Deploy the app to the device and open the Device Explorer app.

6. Once it's loaded (and configured to point to your IoT hub), go to the **Messages To Device** tab, check the **Monitor Feedback Endpoint** option and write your command in the **Message** field. Click on **Send**

	![Sending cloud-to-device message](Images/sending-cloud-to-device-message.png?raw=true)

7. After a few seconds the message will be processed by the device and the LED will turn on in the color you selected. The feedback will also be reflected in the Device Explorer screen after a few seconds.

	![cloud-to-device message received](Images/cloud-to-device-message-received.png?raw=true)


<a name="Summary" />
## Summary
In this lab, you have learned how to create a Universal app that reads from the sensors of a FEZ hat connected to a Raspberry Pi 2 running Windows 10 IoT Core, and upload those readings to an Azure IoT Hub. You also learned how to read and consume the information in the IoT Hub using Power BI to get near real-time analysis of the information gathered from the FEZ hat sensors and to create simple reports and how to consume it using a website. You also saw how to use the IoT Hubs Cloud-To-Device messages feature to send simple commands to your devices.
