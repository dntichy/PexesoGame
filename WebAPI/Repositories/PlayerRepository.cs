using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly PlayerContext _db;

        public PlayerRepository(PlayerContext db)
        {
            _db = db;
        }

        public PlayerWrap CreatePlayer(PlayerWrap player)
        {
            _db.Playersd.Add(player);
            _db.SaveChanges();
            return player;
        }
    }
}