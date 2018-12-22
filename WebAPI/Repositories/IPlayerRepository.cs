using WebAPI.Models;

namespace WebAPI.Repositories
{
    public interface IPlayerRepository
    {
        PlayerWrap CreatePlayer(PlayerWrap player);
        //PlayerWrap GetPlayer(PlayerWrap player);
    }
}