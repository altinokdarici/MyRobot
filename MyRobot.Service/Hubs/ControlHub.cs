using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace MyRobot.Service.Hubs
{
    public class ControlHub : Hub
    {
        public void SendCommand(Command c)
        {
            Clients.Others.CommandReceived(c);
        }
    }

    public enum Command : byte
    {
        Go = 1,
        Back = 2,
        Left = 3,
        Right = 4
    }

}