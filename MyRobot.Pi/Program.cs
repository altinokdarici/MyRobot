using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyRobot.Pi
{
    class Program
    {
        static void Main(string[] args)
        {
            var hubConnection = new HubConnection("http://myrobotservice.azurewebsites.net/");
            hubConnection.StateChanged += HubConnection_StateChanged;
            IHubProxy controlHubProxy = hubConnection.CreateHubProxy("ControlHub");
            controlHubProxy.On<byte>("CommandReceived", command =>
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
    }
}
