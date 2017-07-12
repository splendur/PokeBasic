using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokeBasic.Handler;
using WindowScrape.Types;

namespace PokeBasic.Entities
{
    class Game
    {
        public static List<Poke> OpponentTeam { get; set; }
        public static List<Poke> OwnTeam { get; set; }
        public static Board Board { get; set; }
        public string GameWindowTitle = "BlueStacks App Player";

        public Game(PictureBox pictureBox1)
        {
            var start = DateTime.Now.ToString();
            OpponentTeam = new List<Poke>();
            OwnTeam = new List<Poke>();
            Board = new Board();
            //System.Drawing.Bitmap table = (Bitmap)Bitmap.FromFile(@"c:\users\frick\documents\visual studio 2017\Projects\ConsoleApp1\ConsoleApp1\Images\op1.png");

            HwndObject blueStacksWindow = HwndObject.GetWindowByTitle(GameWindowTitle);
            blueStacksWindow.Location = new Point(10,100);
            SetForegroundWindow(blueStacksWindow.Hwnd);
            blueStacksWindow = HwndObject.GetWindowByTitle(GameWindowTitle);
            var location = blueStacksWindow.Location;
            var size = blueStacksWindow.Size;

            Rectangle formBounds = new Rectangle(location.X+500, location.Y+184, size.Width-680, size.Height-130);

            Bitmap bmp = new Bitmap(formBounds.Width, formBounds.Height);
            SetForegroundWindow(blueStacksWindow.Hwnd);
            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(formBounds.Location, Point.Empty, formBounds.Size);

            //Console.WriteLine("Own Team");
            var idBoard = ConvertToFormat(bmp, PixelFormat.Format24bppRgb);
            OwnTeam = PokeFinder.FindPokes(idBoard, PokeFinder.Teams.Own,pictureBox1);
            //OwnTeam.ForEach(pok => Console.WriteLine(pok.Name));
            ConvertOwnTeamCoordsToBoard();
            //int benchc = 0;
            //foreach (var benchPos in Board._MyBench)
            //{
            //    var OcName = benchPos.Occupant != null ? benchPos.Occupant.Name : "";
            //    //Console.WriteLine($"Bench{benchc++}: {OcName}");
            //}
            //Console.WriteLine("Opponent Team");
            OpponentTeam = PokeFinder.FindPokes(idBoard, PokeFinder.Teams.Opponent, pictureBox1);
            //OpponentTeam.ForEach(pok => Console.WriteLine(pok.Name));
            ConvertOpponentTeamCoordsToBoard();
            //benchc = 0;
            //foreach (var benchPos in Board._OpponentBench)
            //{
            //    var OcName = benchPos.Occupant != null ? benchPos.Occupant.Name : "";
            //    //Console.WriteLine($"Bench{benchc++}: {OcName}");
            //}
            bmp.Save($"board_{DateTime.Now:ddMMyyyyhhmm}.jpg", ImageFormat.Jpeg);

            Console.Write(Board.ToString());
        }

        public string makeDecision()
        {
            var decision = new DecisionMaker(Board);
            var NextMove = decision.ReactorDepth(3);
            return string.Empty;
        }

        public static void ConvertOwnTeamCoordsToBoard()
        {
            var positionedOnBoard = false;
            foreach (var ownPoke in OwnTeam)
            {
                positionedOnBoard = false;
                foreach (var benchPosition in Board._MyBench)
                {
                    if (benchPosition.Occupant == null && 
                        ownPoke.Coords.y < benchPosition.Coords.y &&
                        ownPoke.Coords.IsCoordInRange(benchPosition.Coords, 30))
                    {
                        benchPosition.Occupant = ownPoke;
                        positionedOnBoard = true;
                    }
                }
                if (!positionedOnBoard)
                {
                    foreach (var boardPosition in Board._Board)
                    {
                        if (boardPosition != null &&
                            boardPosition.Occupant == null &&
                            ownPoke.Coords.y < boardPosition.Coords.y &&
                            ownPoke.Coords.IsCoordInRange(boardPosition.Coords, 30))
                        {
                            boardPosition.Occupant = ownPoke;
                            positionedOnBoard = true;
                        }
                    }
                }
            }
        }

        public static void ConvertOpponentTeamCoordsToBoard()
        {
            var positionedOnBoard = false;
            foreach (var opPoke in OpponentTeam)
            {
                positionedOnBoard = false;
                foreach (var benchPosition in Board._OpponentBench)
                {
                    if (benchPosition.Occupant == null &&
                        opPoke.Coords.y < benchPosition.Coords.y &&
                        opPoke.Coords.IsCoordInRange(benchPosition.Coords, 30))
                    {
                        benchPosition.Occupant = opPoke;
                        positionedOnBoard = true;
                    }
                }
                if (!positionedOnBoard)
                {
                    foreach (var boardPosition in Board._Board)
                    {
                        if (boardPosition != null &&
                            boardPosition.Occupant == null &&
                            opPoke.Coords.y < boardPosition.Coords.y &&
                            opPoke.Coords.IsCoordInRange(boardPosition.Coords, 30))
                        {
                            boardPosition.Occupant = opPoke;
                            positionedOnBoard = true;
                        }
                    }
                }
            }
        }

        public static Bitmap ConvertToFormat(Image image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
