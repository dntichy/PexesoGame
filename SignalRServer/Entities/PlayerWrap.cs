using System;
using SignalRServer.Enums;

namespace SignalRServer.Entities
{
    public class PlayerWrap
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public string Opponent { get; set; }
        
        //statistics
        public int Points { get; set; }
        public int TotalMoves { get; set; }
        public int TotalScore { get; set; }
        public GameResult? GameResult { get; set; }
        public GameTypes? GameType { get; set; }
        public DateTime GameStart { get; set; }
        public DateTime GameFinish { get; set; }

    }
}