using WebAPI.Enums;

namespace WebAPI.Models
{
    public class Invitation
    {
        public Player FromPlayer { get; set; }
        public Player ToPlayer { get; set; }
        public GameTypes GameType { get; set; }
    }
}