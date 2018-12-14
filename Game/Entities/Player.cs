using System;

namespace Game.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public Player Opponent { get; set; }
        public bool IsPlaying { get; set; }
        public bool WaitingForMove { get; set; }
        public bool LookingForOpponent { get; set; }
        public DateTime GameStarted { get; set; }
        public string ConnectionId { get; set; }

    }
}