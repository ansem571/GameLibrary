﻿using GameLibrary.Interfaces;
using GameLibrary.Models.Tiles.Special;
using GameLibrary.Models.Tiles.Terrain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace GameLibrary.Models.Maps
{
    public class GlobalMap : IMap
    {
        public int Width { get; }
        public int Height { get; }
        public Dictionary<IPoint, ITile> Grid { get; } = new Dictionary<IPoint, ITile>();
        private string PathToMap { get; set; }
        private Process ImageProcess { get; set; }

        public GlobalMap(int w, int h, string path, List<List<ITile>> tiles)
        {
            Width = w;
            Height = h;
            PathToMap = !string.IsNullOrEmpty(path) ? path : throw new Exception("No path provided");

            SetupGrid(tiles);
        }

        private void SetupGrid(List<List<ITile>> tiles)
        {
            foreach (var tileGroup in tiles)
            {
                foreach (var tile in tileGroup)
                {
                    if (!Grid.ContainsKey(tile.Location))
                        Grid.Add(tile.Location, tile);
                    else
                        Grid[tile.Location] = tile;
                }
            }
        }

        public void OpenMap(IPoint loc)
        {
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            DrawMap(loc);

            ProcessStartInfo psi = new ProcessStartInfo("rundll32.exe",
               string.Format("\"{0}{1}\", ImageView_Fullscreen {2}",
                Environment.Is64BitOperatingSystem ? programFiles.Replace(" (x86)", "") : programFiles,
                @"\Windows Photo Viewer\PhotoViewer.dll",
                PathToMap));
            psi.WindowStyle = ProcessWindowStyle.Normal;

            ImageProcess = Process.Start(psi);
        }
        public void RedrawMap(IPoint loc)
        {
            if (ImageProcess == null || ImageProcess.HasExited)
                OpenMap(loc);
            else
                DrawMap(loc);
        }
        public void CloseMap(IPoint loc)
        {
            DrawMap(loc);
            if (ImageProcess != null && !ImageProcess.HasExited)
            {
                ImageProcess.Kill();
                Thread.Sleep(1000);
                ImageProcess = new Process();
            }
        }

        private void DrawMap(IPoint playerLoc)
        {
            var data = GetMapData(playerLoc);
            var forestCheck = data.ToList().Where(x => x == 10).Count();

            var height = Height;
            var width = Width;

            var scaleHeight = height;
            var scaleWidth = width;

            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
            bmp.UnlockBits(bitmapData);

            bmp.Save(PathToMap);

            bmp = ResizeMap(bmp, 1);
            bmp.Save(PathToMap);
        }

        private Bitmap ResizeMap(Bitmap bmp, int scale = 100)
        {
            scale = scale * bmp.Height > 1000 ? 1000/bmp.Height : scale;

            return new Bitmap(bmp, new Size(bmp.Width * scale, bmp.Height * scale));
        }

        private byte[] GetMapData(IPoint playerLoc)
        {
            List<byte> data = new List<byte>();
            for (int row = Height - 1; row >= 0; row--)
            {
                for (int column = 0; column < Width; column++)
                {
                    if (column == playerLoc.X && row == playerLoc.Y)
                    {
                        data.AddRange(Colors.Player);
                    }
                    else
                    {
                        var loc = (IPoint)Activator.CreateInstance(playerLoc.GetType(), new object[] { column, row });
                        if (!Grid[loc].Visited)
                        {
                            data.AddRange(Colors.FogOfWar);

                        }
                        else
                        {
                            data.AddRange(Colors.GetColorByTileType(Grid[loc]));
                        }
                    }
                    data.Add(255);
                }
            }
            return data.ToArray();
        }

        /// <summary>
        /// Blue, Green, Red, Alpha
        /// </summary>
        internal class Colors
        {
            public static readonly List<byte> FogOfWar = new List<byte> { 0, 0, 0 };
            public static readonly List<byte> Player = new List<byte> { 255, 255, 255 };

            private static readonly List<byte> Plains = new List<byte> { 208, 255, 168 };
            private static readonly List<byte> Desert = new List<byte> { 155, 198, 247 };
            private static readonly List<byte> Forest = new List<byte> { 10, 160, 0 };
            private static readonly List<byte> Seaside = new List<byte> { 255, 255, 10 };
            private static readonly List<byte> Mountain = new List<byte> { 0, 82, 145 };

            private static readonly List<byte> Enemy = new List<byte> { 28, 28, 216 };
            private static readonly List<byte> Dungeon = new List<byte> { 140, 140, 140 };
            private static readonly List<byte> Town = new List<byte> { 239, 198, 141 };
            private static readonly List<byte> POI = new List<byte> { 46, 247, 223 };


            public static List<byte> GetColorByTileType(ITile tile)
            {
                if (tile is PlainsTile)
                    return Plains;
                if (tile is DesertTile)
                    return Desert;
                if (tile is ForestTile)
                    return Forest;
                if (tile is SeasideTile)
                    return Seaside;
                if (tile is MountainTile)
                    return Mountain;

                if (tile is EnemyTile)
                    return Enemy;
                if (tile is DungeonTile)
                    return Dungeon;
                if (tile is TownTile)
                    return Town;
                if (tile is POITile)
                    return POI;

                return FogOfWar;
            }
        }
    }
}
