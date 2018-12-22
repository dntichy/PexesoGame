using System;
using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Models
{
    public class PlayerWrap
    {
        [Key]
        public string ConnectionId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Opponent { get; set; }

        //statistics
        [Required]
        public int Points { get; set; }
        [Required]
        public int TotalMoves { get; set; }
        [Required]
        public GameResult? GameResult { get; set; }
        [Required]
        public GameTypes? GameType { get; set; }
        [Required]
        public DateTime GameStart { get; set; }
        [Required]
        public DateTime GameFinish { get; set; }

    }
}