using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRServer.Enums;

namespace SignalRServer
{
    public class GameHub : Hub
    {
        private static readonly object SyncRoot = new object();
        private static int _gamesPlayed = 0;
        private static readonly List<Player> clients = new List<Player>();
        private static readonly List<Pexeso> Games = new List<Pexeso>();


        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("DISCONNECTED");

            var Player = clients.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            clients.RemoveAll(p => p.ConnectionId == Context.ConnectionId);

            if (Player?.Opponent != null)
            {
                Player.Opponent.Reinitialize();
                Games.RemoveAll(n =>
                    n.Player1.ConnectionId == Context.ConnectionId || n.Player2.ConnectionId == Context.ConnectionId);
                //send that opponent has disconnected
                return Clients.Client(Player.Opponent.ConnectionId).opponentDisconnected(Player.Name);
            }

            return this.RefreshPlayers();
        }

        public override Task OnConnected()
        {
            Console.WriteLine("connected");
            //return SendStatsUpdate();
            return base.OnConnected();
        }

        public Task SendStatsUpdate()
        {
            return Clients.All.refreshAmountOfPlayers(new
            {
                totalGamesPlayed = _gamesPlayed,
                amountOfGames = Games.Count,
                amountOfClients = clients.Count
            });
        }

        public void RegisterClient(string data)
        {
            lock (SyncRoot)
            {
                var Player = clients.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

                //user already exists
                if (clients.Exists(n => n.Name == data))
                {
                    Clients.Client(Context.ConnectionId).registerCantCompleted();
                    return;
                }

                //if (Player == null)
                //{
                Player = new Player {ConnectionId = Context.ConnectionId, Name = data};
                clients.Add(Player);
                //}

                Player.IsPlaying = false;
            }

            //SendStatsUpdate();
            Clients.Client(Context.ConnectionId).registerComplete();
        }

        public Task RefreshPlayers()
        {
            return Clients.All.listOfPlayers(clients);
        }

        public Task SearchPlayer(string name)
        {
            return Clients.Client(Context.ConnectionId)
                .listOfSearchedPlayers(clients.FindAll(n => n.Name.ToLower().Contains(name.ToLower())));
        }


        public void ChallengePlayer(string name, string gameType)
        {
            //je hrac v hre? => posli fail status

            //posli mu challenge, nastav obom isPlaying na true
            //nastav openentov jeden druhemu
            var challenger = clients.Find(c => c.ConnectionId == Context.ConnectionId);
            var challengee = clients.Find(c => c.Name == name);

            GameTypes GameType = GameTypes.TriXDva;
            switch (gameType)
            {
                case "3x2":
                    GameType = GameTypes.TriXDva;
                    break;
                case "4x3":
                    GameType = GameTypes.StyriXTri;
                    break;
                case "4x4":
                    GameType = GameTypes.StyriXStyri;
                    break;
                case "5x4":
                    GameType = GameTypes.PatXStyri;
                    break;
                case "6x5":
                    GameType = GameTypes.SestXPat;
                    break;
                case "6x6":
                    GameType = GameTypes.SedemXSest;
                    break;
                case "7x6":
                    GameType = GameTypes.SedemXSest;
                    break;
                case "8x7":
                    GameType = GameTypes.OsemXSedem;
                    break;
                case "8x8":
                    GameType = GameTypes.OsemXOsem;
                    break;
            }

            if (!challengee.HasInvitation)
            {
                Invitation newInvitation = new Invitation()
                {
                    FromPlayer = challenger,
                    ToPlayer = challengee,
                    GameType = GameType
                };
                challenger.HasInvitation = true;
                challengee.HasInvitation = true;
                challenger.Invitation = newInvitation;
                challengee.Invitation = newInvitation;

                Clients.Client(challengee.ConnectionId).gotInvitation(challenger.Name);
            }
            else
            {
                Clients.Client(Context.ConnectionId).challengePlayerFailed(name);
            }


            //challengee.Opponent = challenger;
            //challengee.IsPlaying = 
            //challenger.Opponent = challengee;
            //return Clients.All.x();
        }

        public void RejectInvitation(string name)
        {
            var challengee = clients.Find(c => c.ConnectionId == Context.ConnectionId);
            var challenger = clients.Find(c => c.Name == name);
            challengee.HasInvitation = false;
            challenger.HasInvitation = false;
            challenger.Invitation = null;
            challengee.Invitation = null;
            ////v pripade, že jeden z nich zruší žiadosť, nastav na isPlaying false pre oboch a informuj ich o zrušení

            Clients.Client(challenger.ConnectionId).challengePlayerFailed(challengee.Name);
        }

        //invitation od uzivatela name accepted
        public void AcceptInvitation(string name)
        {
            var challengee = clients.Find(c => c.ConnectionId == Context.ConnectionId); //vyzyvany
            var challenger = clients.Find(c => c.Name == name); //vyzyvatel

            Pexeso newPexesoGame = new Pexeso()
            {
                Player1 = challenger,
                Player2 = challengee,
                GameType = challengee.Invitation.GameType,
                GameStart = new DateTime()
                
            };

            Games.Add(newPexesoGame);

            challenger.Opponent = challengee;
            challengee.Opponent = challenger;
            challengee.GamePexeso = newPexesoGame;
            challenger.GamePexeso = newPexesoGame;

            Clients.Client(challenger.ConnectionId).challengeAccepted();

            GetMultipleValue(newPexesoGame.GameType, out var a, out var b);
            Clients.Clients(new[] {challenger.ConnectionId, challengee.ConnectionId}).createGameScenario(a, b);

            challengee.Moving = true;
            challenger.Moving = false;

            Clients.Client(challengee.ConnectionId).move();
            Clients.Client(challenger.ConnectionId).waitForMove();
        }

        public void Moved(int a, int b)
        {
            var game = Games.FirstOrDefault(x =>
                x.Player1.ConnectionId == Context.ConnectionId || x.Player2.ConnectionId == Context.ConnectionId);
            var theOneThatMoves = clients.Find(n => n.ConnectionId == Context.ConnectionId);
            var theOpponenet = clients.Find(n => n.ConnectionId == Context.ConnectionId).Opponent;


            theOneThatMoves.MoveCounter++;

            //if 1st move
            if (theOneThatMoves.MoveCounter < 2)
            {
                theOpponenet.MoveCounter++;
                Clients.Client(Context.ConnectionId).move();
            }
            else
            {
                theOneThatMoves.MoveCounter = 0;
                Clients.Client(Context.ConnectionId).waitForMove();
            }
        }


        private void GetMultipleValue(GameTypes type, out int a, out int b)
        {
            a = -1;
            b = -1;

            switch (type)
            {
                case GameTypes.TriXDva:
                    a = 3;
                    b = 2;
                    break;
                case GameTypes.StyriXTri:
                    a = 4;
                    b = 3;
                    break;
                case GameTypes.StyriXStyri:
                    a = 4;
                    b = 4;
                    break;
                case GameTypes.PatXStyri:
                    a = 5;
                    b = 4;
                    break;
                case GameTypes.SestXPat:
                    a = 6;
                    b = 5;
                    break;
                case GameTypes.SestXSest:
                    a = 6;
                    b = 6;
                    break;
                case GameTypes.SedemXSest:
                    a = 7;
                    b = 6;
                    break;
                case GameTypes.OsemXSedem:
                    a = 8;
                    b = 7;
                    break;
                case GameTypes.OsemXOsem:
                    a = 8;
                    b = 8;
                    break;
            }
        }
    }
}