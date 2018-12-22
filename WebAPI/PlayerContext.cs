using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI
{
    public class PlayerContext : DbContext
    {
        private readonly IPlayerRepository _orders;

        public PlayerContext(DbContextOptions<PlayerContext> options)
            : base(options)
        {
        }

        public DbSet<PlayerWrap> Playersd { get; set; }
    }
}
