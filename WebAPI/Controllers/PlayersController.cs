using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Enums;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {

        private readonly IPlayerRepository _playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }


        // GET: api/Players
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Players/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Players
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{

        //    var pl = new PlayerWrap()
        //    {
        //        Name = "12",
        //        Opponent = "2",
        //        GameResult = GameResult.DRAW,
        //        TotalMoves = 2,
        //        ConnectionId = "2114124",
        //        TotalScore = 1254,
        //        GameFinish = new DateTime(),
        //        GameStart = new DateTime(),
        //        GameType = GameTypes.OsemXOsem,
        //        Points = 2
        //    };
        //    _playerRepository.CreatePlayer(pl);
        //}

        //[HttpPost]
        public void Post([FromBody] PlayerWrap value)
        {
            _playerRepository.CreatePlayer(value);
        }

        // PUT: api/Players/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


}
