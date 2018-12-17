using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalRServer.Enums;

namespace SignalRServer
{
    public class Invitation
    {
        public Player FromPlayer { get; set; }
        public Player ToPlayer { get; set; }
        public GameTypes GameType { get; set; }
    }
}