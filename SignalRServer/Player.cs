using System;
using System.Collections.Generic;
using SignalRServer.Enums;

namespace SignalRServer
{
    public class Player
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public Player Opponent { get; set; }
        public bool HasInvitation { get; set; }
        public bool IsPlaying { get; set; }
        public Invitation Invitation { get; set; }
        public Pexeso GamePexeso { get; set; }

        public bool WaitingForMove { get; set; }
        public bool Moving { get; set; }
        public int MoveCounter { get; set; }


        public void Reinitialize()
        {
            HasInvitation = false;
            IsPlaying = false;
            Invitation = null;
            GamePexeso = null;
            Moving = false;
            WaitingForMove = false;
            Opponent = null;
        }
    }
}