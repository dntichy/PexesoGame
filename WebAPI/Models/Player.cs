using WebAPI.Enums;

namespace WebAPI.Models
{
    public class Player
    {
        //metadata
        public string ConnectionId { get; set; }
        public string Name { get; set; }


        public Player Opponent { get; set; }
        public bool HasInvitation { get; set; }
        public bool IsPlaying { get; set; }
        public Invitation Invitation { get; set; }
        public Pexeso GamePexeso { get; set; }
        public bool Moving { get; set; }
        public int MoveCounter { get; set; }

        //statistics
        public int Points { get; set; }
        public int TotalMoves { get; set; }
        public int TotalScore { get; set; }
        public GameResult? GameResult { get; set; }


        public void Reinitialize()
        {
            GameResult = null;
            TotalMoves = 0;
            TotalScore = 0;
            Points = 0;

            HasInvitation = false;
            IsPlaying = false;
            Invitation = null;
            GamePexeso = null;
            Moving = false;
            Opponent = null;
            MoveCounter = 0;
        }
    }
}