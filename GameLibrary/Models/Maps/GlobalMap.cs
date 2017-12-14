using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using GameLibrary.Models.Tiles.Special;
using GameLibrary.Models.Tiles.Terrain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ImageDisplayApp;
using System.Threading.Tasks;

namespace GameLibrary.Models.Maps
{
    public class GlobalMap : IMap
    {
        private int Width;
        private int Height;
        private Dictionary<IPoint, ITile> Grid = new Dictionary<IPoint, ITile>();
        private string PathToMap { get; set; }
        private string PathToApp { get; set; }
        //private Process ImageProcess { get; set; }
        private string path1;
        private string path2;
        private string originalPath { get; set; }
        Global_Map map;
        Task t;

        public GlobalMap(int w, int h, string mapPath, string appPath, List<List<ITile>> tiles)
        {
            Width = w;
            Height = h;
            PathToMap = mapPath ?? throw new ArgumentNullException(nameof(mapPath), "No path provided");
            PathToApp = appPath ?? throw new ArgumentNullException(nameof(appPath), "No path provided");

            path1 = PathToMap.Replace("map.png", "map1.png");
            path2 = PathToMap.Replace("map.png", "map2.png");
            originalPath = PathToMap;
            PathToMap = path1;
            SetupGrid(tiles);
            map = new Global_Map(PathToMap);
        }

        private void SetupGrid(List<List<ITile>> tiles)
        {
            if (tiles == null)
                throw new ArgumentNullException(nameof(tiles));
            if (!tiles.Any())
                throw new ArgumentException("Tiles collection is empty", nameof(tiles));
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
            DrawMap(loc);

            t = new Task(() => Application.Run(map));
            t.Start();

            //var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            //ImageProcess = Process.GetProcessesByName(map.Name).First();

            //ProcessStartInfo psi = new ProcessStartInfo(PathToApp);
            //psi.WindowStyle = ProcessWindowStyle.Normal;

            //ImageProcess = Process.Start(psi);
        }
        public void RedrawMap(IPoint loc)
        {
            if (t.Status == TaskStatus.RanToCompletion)
            {
                t.Dispose();
                map = null;

                if (File.Exists(PathToMap))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(PathToMap);
                }
                map = new Global_Map(PathToMap);
                OpenMap(loc);
            }
            else
            {
                PathToMap = PathToMap == path1 ? path2 : path1;

                if (File.Exists(PathToMap))
                {
                    File.Delete(PathToMap);
                }
                DrawMap(loc);
                map.Refresh(PathToMap);
            }
        }
        public void CloseMap(IPoint loc)
        {
            PathToMap = originalPath;
            DrawMap(loc);
        }

        private void DrawMap(IPoint playerLoc)
        {
            var data = GetMapData(playerLoc);

            CreateMap(data, Width, Height);

            ResizeMapProper(data, 50);
        }

        private void CreateMap(byte[] data, int width, int height)
        {
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, unmanagedPointer, data.Length);
            using (Bitmap bmp = new Bitmap(width, height, width * 4, PixelFormat.Format32bppRgb, unmanagedPointer))
            {
                try
                {
                    bmp.Save(PathToMap);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.ReadLine();
                }
            }
            Marshal.FreeHGlobal(unmanagedPointer);
        }


        private void CopyToNewLocation(Bitmap newImage)
        {
            var originalPath = PathToMap;
            var tempPath = PathToMap.Replace(".png", "temp.png");

            File.Move(originalPath, tempPath);
            try
            {
                newImage.Save(originalPath, ImageFormat.Bmp);
            }
            catch
            {
                File.Move(originalPath, tempPath);
                throw;
            }
            finally
            {
                newImage.Dispose();
            }
            File.Delete(tempPath);
        }
        private void ResizeMapProper(byte[] data, int scale = 10)
        {
            List<byte> newData = new List<byte>();
            const int maxSize = 600;
            var ratioX = scale * Width > maxSize ? maxSize / Width : scale;
            var ratioY = scale * Height > maxSize ? maxSize / Height : scale;

            scale = ratioX < ratioY ? ratioX : ratioY;
            if (scale <= 1)
                return;

            var newHeight = Height * scale;
            var newWidth = Width * scale;

            int nextRowByteIndex = 0;
            int rowByteIndex = 0;
            int currentByteIndex = 0;

            const int skip = 4;
            for (var r = Height - 1; r >= 0; r--)
            {
                rowByteIndex = nextRowByteIndex;
                for (var scaledRow = 0; scaledRow < scale; scaledRow++)
                {
                    currentByteIndex = rowByteIndex;
                    for (var c = 0; c < Width; c++)
                    {
                        for (var scaledColumn = 0; scaledColumn < scale; scaledColumn++)
                        {
                            List<byte> color = data.ToList().GetRange(currentByteIndex, skip);
                            newData.AddRange(color);
                        }
                        currentByteIndex += skip;
                    }
                    nextRowByteIndex = currentByteIndex;
                }
            }

            CreateMap(newData.ToArray(), newHeight, newWidth);
        }

        private Bitmap ResizeMap(Bitmap bmp, int scale = 100)
        {
            scale = scale * bmp.Height > 1000 ? 1000 / bmp.Height : scale;

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

        public float GetWidth()
        {
            return Width;
        }

        public float GetHeight()
        {
            return Height;
        }

        public Dictionary<IPoint, ITile> GetMapGrid()
        {
            return Grid;
        }

        public ITile GetTileByLocation(IPoint point)
        {
            return Grid[point];
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
