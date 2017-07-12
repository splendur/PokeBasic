using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Math;
using PokeBasic.Entities;
using Image = System.Drawing.Image;
using PokeBasic.Model;

namespace PokeBasic.Handler
{
    static class PokeFinder
    {
        public enum Teams
        {
            Own,
            Opponent
        }

        private static readonly List<Color> TeamColors =
            new List<Color>() {Color.White, Color.Blue, Color.Green, Color.Red, Color.BlueViolet, Color.Orange};

        public static List<Poke> FindPokes(Bitmap table, Teams team, PictureBox pictureBox1 = null)
        {
            var matches = new List<TemplateMatch>();
            var result = new List<Poke>();
            var pokeSpriteType = team.Equals(Teams.Own) ? @"c:\users\frick\documents\visual studio 2017\Projects\PokeBasic\PokeBasic\Pokes\Back\" : @"c:\users\frick\documents\visual studio 2017\Projects\PokeBasic\PokeBasic\Pokes\Face\";
            var files = Directory.GetFiles(pokeSpriteType);
            int pokeFoundCounter = 0;
            foreach (var fileName in files)
            {
                var pokePositions = IsPokeInImage(table, fileName).ToList();
                matches.AddRange(pokePositions);
                foreach (var match in pokePositions)
                {
                    var tPoke = new Poke(Path.GetFileNameWithoutExtension(fileName), match.Rectangle.X, match.Rectangle.Y);
                    var ttPoke = DBHandler.getPoke(tPoke);
                    ttPoke.Coords = tPoke.Coords;
                    result.Add(ttPoke);
                }
            }
            BitmapData data = table.LockBits(
                new Rectangle(0, 0, table.Width, table.Height),
                ImageLockMode.ReadWrite, table.PixelFormat);
            foreach (TemplateMatch m in matches)
            {
                Drawing.Rectangle(data, m.Rectangle, TeamColors[(pokeFoundCounter > TeamColors.Count-1) ? TeamColors.Count - 1 : pokeFoundCounter++]);
            }
            if (pictureBox1 != null)
            {
                pictureBox1.Height = table.Height;
                pictureBox1.Width = table.Width;
                pictureBox1.Image = table;
            }
            table.UnlockBits(data);

            return result;
        }
        
        public static TemplateMatch[] IsPokeInImage(Bitmap table, string pokeSpritePath)
        {
            System.Drawing.Bitmap pokeSprite = (Bitmap)Bitmap.FromFile(pokeSpritePath);
            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.921f);
            var result = tm.ProcessImage(table, pokeSprite);

            return result;
        }

        
    }
}
