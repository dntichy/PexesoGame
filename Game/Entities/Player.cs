namespace Game.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public Player Opponent { get; set; }
        public bool HasInvitation { get; set; }
        public bool IsPlaying { get; set; }
        public Invitation Invitation { get; set; }
        public bool Moving { get; set; }
        public int MoveCounter { get; set; }
        public int Points { get; set; }

        public void Reinitialize()
        {
            HasInvitation = false;
            IsPlaying = false;
            Invitation = null;
           
            Moving = false;
            Opponent = null;
            Points = 0;
            MoveCounter = 0;
        }
    }
}