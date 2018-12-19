using System;
using System.IO;
using Game.Enums;

namespace Game.Entities
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

        public int MaxPointsForGame;

        public MoveKeeper MoveKeeper { get; set; }

        public Pexeso()
        {
            Console.WriteLine("NEW PEx");
            MoveKeeper = new MoveKeeper();
        }

        public void InitializeGameField()
        {
            switch (GameType)
            {
                case GameTypes.OsemXOsem:
                    GameField = new Picture[8, 8];
                    MaxPointsForGame = 64;
                    break;

                case GameTypes.SedemXSest:
                    GameField = new Picture[7, 6];
                    MaxPointsForGame = 48;
                    break;

                case GameTypes.OsemXSedem:
                    GameField = new Picture[8, 7];
                    MaxPointsForGame = 56;
                    break;
                case GameTypes.PatXStyri:
                    GameField = new Picture[5, 4];
                    MaxPointsForGame = 20;
                    break;

                case GameTypes.SestXPat:
                    GameField = new Picture[6, 5];
                    MaxPointsForGame = 30;
                    break;

                case GameTypes.SestXSest:
                    GameField = new Picture[6, 6];
                    MaxPointsForGame = 36;
                    break;

                case GameTypes.StyriXStyri:
                    GameField = new Picture[4, 4];
                    MaxPointsForGame = 16;
                    break;

                case GameTypes.StyriXTri:
                    GameField = new Picture[4, 3];
                    MaxPointsForGame = 12;
                    break;

                case GameTypes.TriXDva:
                    GameField = new Picture[3, 2];
                    MaxPointsForGame = 6;
                    break;
            }

            FillWithPictures(GameField);
        }


        private void FillWithPictures(Picture[,] gameField)
        {
            var row = gameField.GetLength(0);
            var column = gameField.GetLength(1);
            int counter = 0;
            Random rand = new Random();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (gameField[i, j] == null)
                    {
                        string path = Path.Combine(Environment.CurrentDirectory,
                            @"Pictures\" + counter + ".jpg");
                        var imageData = File.ReadAllBytes(path);

                        gameField[i, j] = new Picture()
                        {
                            Image = imageData
                        };

                        bool success = false;
                        while (!success)
                        {
                            var a = rand.Next(0, row);
                            var b = rand.Next(0, column);

                            if (gameField[a, b] == null)
                            {
                                gameField[a, b] = new Picture()
                                {
                                    Image = imageData
                                };
                                success = true;
                                counter++;
                            }
                        }
                    }
                }
            }
        }
    }

    public class MoveKeeper
    {
        public int FirstA { get; set; }
        public int FirstB { get; set; }
        public int SecondA { get; set; }
        public int SecondB { get; set; }
    }

    public class Picture
    {
        public byte[] Image;
    }
}