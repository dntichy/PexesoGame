using System;
using System.IO;
using System.Reflection;
using System.Web;
using SignalRServer.Enums;

namespace SignalRServer
{
    public class Pexeso
    {
        public bool IsGameOver { get; private set; }

        public bool IsDraw { get; private set; }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        public GameTypes GameType { get; set; }

        public DateTime GameStart { get; set; }

        public Picture[,] GameField;

        public Pexeso()
        {
            Console.WriteLine("NEW PEx");

            switch (GameType)
            {
                case GameTypes.OsemXOsem:
                    GameField = new Picture[8, 8];
                    break;

                case GameTypes.SedemXSest:
                    GameField = new Picture[7, 6];
                    break;

                case GameTypes.OsemXSedem:
                    GameField = new Picture[8, 7];
                    break;
                case GameTypes.PatXStyri:
                    GameField = new Picture[5, 4];
                    break;

                case GameTypes.SestXPat:
                    GameField = new Picture[6, 5];
                    break;

                case GameTypes.SestXSest:
                    GameField = new Picture[6, 6];
                    break;

                case GameTypes.StyriXStyri:
                    GameField = new Picture[4, 4];
                    break;

                case GameTypes.StyriXTri:
                    GameField = new Picture[4, 3];
                    break;

                case GameTypes.TriXDva:
                    GameField = new Picture[3, 2];
                    break;
            }

            FillWithPictures(GameField);
        }

        private void FillWithPictures(Picture[,] gameField)
        {


            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"Pictures\0.jpg");

            var imageData = File.ReadAllBytes(path);

            gameField[0, 0] = new Picture()
            {
                Image = imageData
            };
        }
    }

    public class Picture
    {
        public byte[] Image;
    }
}