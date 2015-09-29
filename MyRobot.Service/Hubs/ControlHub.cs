using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MyRobot.Models;

namespace MyRobot.Service.Hubs
{
    public class ControlHub : Hub
    {
        public void SendCommand(Command c)
        {
            Clients.Others.CommandReceived(c);
        }
    }


}