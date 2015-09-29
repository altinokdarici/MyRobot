using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyRobot.Models
{
    public enum Command : byte
    {
        NoOp = 0,
        Go = 1,
        Back = 2,
        Left = 3,
        Right = 4
    }
}
