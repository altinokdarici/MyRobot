using System;
using WiringPi;
using Microsoft.AspNet.SignalR.Client;
using System.Threading;
using System.Diagnostics;


namespace Pi
{
	class MainClass
	{
		
		readonly static int pinLeftPositive = 0;
		readonly static int pinLeftNegative = 2;
		readonly static int pinRightPositive = 3;
		readonly static int pinRightNegative = 4;

		static IHubProxy controlHubProxy = null;
		static HubConnection hubConnection = null;

		static void Main (string[] args)
		{
			WiringPi.Init.WiringPiSetupGpio ();
			if (WiringPi.Init.WiringPiSetup () == -1) {
				throw new Exception ("Setup failed");
			}

			SoftPwm.Create (pinLeftPositive, 0, 100);
			SoftPwm.Create (pinLeftNegative, 0, 100);
			SoftPwm.Create (pinRightPositive, 0, 100);
			SoftPwm.Create (pinRightNegative, 0, 100);


			hubConnection = new HubConnection ("http://myrobotservice.azurewebsites.net/");
			controlHubProxy = hubConnection.CreateHubProxy ("ControlHub");
			controlHubProxy.On<Command> ("CommandReceived", command => {
				Console.WriteLine (string.Format ("{0} at {1}\t {2}", command, DateTime.Now, DateTime.Now.Millisecond));
				ProccessCommand	(command);
			});
			hubConnection.Start ().Wait ();
			while (true) {
				Distance ();

			}
			//	Console.Read();


		}


		private static void ProccessCommand (Command c)
		{
			switch (c) {
			case Command.NoOp:
				SoftPwm.Write (pinLeftPositive, 0);
				SoftPwm.Write (pinLeftNegative, 0);
				SoftPwm.Write (pinRightPositive, 0);
				SoftPwm.Write (pinRightNegative, 0);
				break;
			case Command.Go:
				SoftPwm.Write (pinLeftPositive, 100);
				SoftPwm.Write (pinLeftNegative, 0);
				SoftPwm.Write (pinRightPositive, 100);
				SoftPwm.Write (pinRightNegative, 0);
				break;
			case Command.Back:
				SoftPwm.Write (pinLeftPositive, 0);
				SoftPwm.Write (pinLeftNegative, 100);
				SoftPwm.Write (pinRightPositive, 0);
				SoftPwm.Write (pinRightNegative, 100);
				break;
			case Command.Left:
				SoftPwm.Write (pinLeftPositive, 0);
				SoftPwm.Write (pinLeftNegative, 0);
				SoftPwm.Write (pinRightPositive, 100);
				SoftPwm.Write (pinRightNegative, 0);
				break;
			case Command.Right:
				SoftPwm.Write (pinLeftPositive, 100);
				SoftPwm.Write (pinLeftNegative, 0);
				SoftPwm.Write (pinRightPositive, 0);
				SoftPwm.Write (pinRightNegative, 0);
				break;
			default:
				break;
			}
		}

		private static void Distance ()
		{
			ProcessStartInfo psi = new ProcessStartInfo();
			psi.FileName = "/home/pi/Desktop/pi/range.sh";
			psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;
			psi.CreateNoWindow =false;
			Process p = Process.Start(psi);
			string distance = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			Console.WriteLine(p.HasExited.ToString() + " "+ p.ExitCode.ToString() + " " + distance);


		/*	System.Console.WriteLine ("flag 1" + p.ExitCode.ToString ());
			string distance =	System.IO.File.ReadAllText ("/home/pi/Desktop/pi/distance.txt");
			Console.WriteLine ("Distance " + distance);
		*/	controlHubProxy.Invoke ("SendDistance", new object[]{ distance });
	


		
		}

	}

	public enum Command : byte
	{
		NoOp = 0,
		Go = 1,
		Back = 2,
		Left = 3,
		Right = 4
	}

}
