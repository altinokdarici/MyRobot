using Microsoft.AspNet.SignalR.Client;
using MyRobot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WiringPi;

namespace MyRobot.Pi
{
    class Program
    {
        readonly static int pinLeftPositive = 0;
        readonly static int pinLeftNegative = 0;
        readonly static int pinRightPositive = 0;
        readonly static int pinRightNegative = 0;


        static void Main(string[] args)
        {
            if (WiringPi.Init.WiringPiSetup() == -1)
            {
                throw new Exception("Setup failed");
            }

            SoftPwm.Create(pinLeftPositive, 0, 100);
            SoftPwm.Create(pinLeftNegative, 0, 100);
            SoftPwm.Create(pinRightPositive, 0, 100);
            SoftPwm.Create(pinRightNegative, 0, 100);


            var hubConnection = new HubConnection("http://myrobotservice.azurewebsites.net/");
            hubConnection.StateChanged += HubConnection_StateChanged;
            IHubProxy controlHubProxy = hubConnection.CreateHubProxy("ControlHub");
            controlHubProxy.On<Models.Command>("CommandReceived", command =>
            {
                Console.WriteLine(string.Format("{0} at {1}\t {2}", command, DateTime.Now, DateTime.Now.Millisecond));
            });
            ServicePointManager.DefaultConnectionLimit = 10;
            hubConnection.Start().Wait();
            Console.Read();
        }

        private static void HubConnection_StateChanged(StateChange obj)
        {
            Console.WriteLine(string.Format("{0} at {1}", obj.NewState.ToString(), DateTime.Now));
        }
        private static void ProccessCommand(Command c)
        {
            switch (c)
            {
                case Command.NoOp:
                    SoftPwm.Write(pinLeftPositive, 0);
                    SoftPwm.Write(pinLeftNegative, 0);
                    SoftPwm.Write(pinRightPositive, 0);
                    SoftPwm.Write(pinRightNegative, 0);
                    break;
                case Command.Go:
                    SoftPwm.Write(pinLeftPositive, 1);
                    SoftPwm.Write(pinLeftNegative, 0);
                    SoftPwm.Write(pinRightPositive, 1);
                    SoftPwm.Write(pinRightNegative, 0);
                    break;
                case Command.Back:
                    SoftPwm.Write(pinLeftPositive, 0);
                    SoftPwm.Write(pinLeftNegative, 1);
                    SoftPwm.Write(pinRightPositive, 0);
                    SoftPwm.Write(pinRightNegative, 1);
                    break;
                case Command.Left:
                    SoftPwm.Write(pinLeftPositive, 1);
                    SoftPwm.Write(pinLeftNegative, 0);
                    SoftPwm.Write(pinRightPositive, 0);
                    SoftPwm.Write(pinRightNegative, 0);
                    break;
                case Command.Right:
                    SoftPwm.Write(pinLeftPositive, 0);
                    SoftPwm.Write(pinLeftNegative, 0);
                    SoftPwm.Write(pinRightPositive, 1);
                    SoftPwm.Write(pinRightNegative, 0);
                    break;
                default:
                    break;
            }
        }
    }
}

