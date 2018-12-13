using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SignalRServer
{
    public class GameHub : Hub
    {
        private static object _syncRoot = new object();
        private static int _gamesPlayed = 0;
        private static readonly List<Player> clients = new List<Player>();
        private static readonly List<Pexeso> Games = new List<Pexeso>();

        public override Task OnConnected()
        {
            Console.WriteLine("connected");
            //return SendStatsUpdate();
            return base.OnConnected();
        }
        public Task SendStatsUpdate()
        {
            
            return Clients.All.refreshAmountOfPlayers(new {
                totalGamesPlayed = _gamesPlayed,
                amountOfGames = Games.Count,
                amountOfClients = clients.Count });
        }

        public void RegisterClient(string data)
        {
            lock (_syncRoot)
            {
                var Player = clients.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                if (Player == null)
                {
                    Player = new Player { ConnectionId = Context.ConnectionId, Name = data };
                    clients.Add(Player);
                }

                Player.IsPlaying = false;
            }

            SendStatsUpdate();
            Clients.Client(Context.ConnectionId).registerComplete();
        }


        public void Play(int position)
        {
            // Find the game where there is a player1 and player2 and either of them have the current connection id
            var game = Games.FirstOrDefault(x => x.Player1.ConnectionId == Context.ConnectionId || x.Player2.ConnectionId == Context.ConnectionId);

            if (game == null || game.IsGameOver) return;

            int marker = 0;

            // Detect if the player connected is player 1 or player 2
            if (game.Player2.ConnectionId == Context.ConnectionId)
            {
                marker = 1;
            }
            var player = marker == 0 ? game.Player1 : game.Player2;

            // If the player is waiting for the opponent but still tried to make a move, just return
            if (player.WaitingForMove) return;

            // Notify both players that a marker has been placed
            Clients.Client(game.Player1.ConnectionId).addMarkerPlacement(new Program.GameInformation { OpponentName = player.Name, MarkerPosition = position });
            Clients.Client(game.Player2.ConnectionId).addMarkerPlacement(new Program.GameInformation { OpponentName = player.Name, MarkerPosition = position });

            // Place the marker and look for a winner
            if (game.Play(marker, position))
            {
                Games.Remove(game);
                _gamesPlayed += 1;
                Clients.Client(game.Player1.ConnectionId).gameOver(player.Name);
                Clients.Client(game.Player2.ConnectionId).gameOver(player.Name);
            }

            // If it's a draw notify the players that the game is over
            if (game.IsGameOver && game.IsDraw)
            {
                Games.Remove(game);
                _gamesPlayed += 1;
                Clients.Client(game.Player1.ConnectionId).gameOver("It's a draw!");
                Clients.Client(game.Player2.ConnectionId).gameOver("It's a draw!");
            }

            if (!game.IsGameOver)
            {
                player.WaitingForMove = !player.WaitingForMove;
                player.Opponent.WaitingForMove = !player.Opponent.WaitingForMove;

                Clients.Client(player.Opponent.ConnectionId).waitingForMarkerPlacement(player.Name);
            }

            SendStatsUpdate();
        }
    }
}
