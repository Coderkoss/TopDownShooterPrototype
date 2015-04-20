//#define Debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;


namespace TopDownShooterPrototype
{
    //tilmap has one job it makes a map of tiles aka a graph of tiles
    public class TileMap
    { 
        const int TILESIZE = 64;
        int[,] tilemap;
        int tileMapWidth;
        int tileMapHeight;
        Color collisionColor;
        List<Tile> tiles;
        public List<Tile> Tiles 
        {
            get { return tiles; }
            set { tiles = value; }
        }
        public int[,] Map 
        {
            get { return tilemap; }
            set { tilemap = value; }
        }

        public TileMap(int [,] tileMap)
        {
            collisionColor = Color.Gray;
            tiles = new List<Tile>();
            this.tilemap = tileMap;
            tileMapWidth = Map.GetLength(1);
            tileMapHeight = Map.GetLength(0);

            for (int x = 0; x < tileMapWidth; x++)
            {
                for (int y = 0; y < tileMapHeight; y++)
                {
                    tiles.Add(new Tile(new Vector2(x * TILESIZE, y * TILESIZE), tilemap[y, x], Color.White));
                }
            }
           
        }
        public static int[,] Load(string filename) 
        {
            int[,] tempMap;
            bool readingLayout = false;
            List<List<int>> tempLayout = new List<List<int>>();
            using (StreamReader reader = new StreamReader(filename)) 
            {
                while (!reader.EndOfStream) 
                {
                    string line = reader.ReadLine().Trim();
                    if (string.IsNullOrEmpty(line))
                        continue;
                    if (line.Contains("[Layout]")) 
                    {
                        readingLayout = true;
                    }
                    else if (readingLayout) 
                    {
                        List<int> row = new List<int>();
                        string []cells = line.Split(' ');
                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }
                        tempLayout.Add(row);                   
                    }
                }
            }
            int width = tempLayout[0].Count;
            int height = tempLayout.Count;
            tempMap = new int[height, width];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tempMap[y, x] = SetCellIndex(tempLayout[y][x]);
                }
            }
            return tempMap;
        }
        public static int SetCellIndex(int cellIndex) 
        {
            return cellIndex;
        }
        public void Update() 
        {
            tiles.Clear();
            for (int x = 0; x < tileMapWidth; x++)
            {
                for (int y = 0; y < tileMapHeight; y++)
                {
                    tiles.Add(new Tile(new Vector2(x * TILESIZE, y * TILESIZE), tilemap[y, x], Color.White));
                }
            }
        }
        public void TileMapDraw(SpriteBatch spriteBatch)
        {
            
            foreach (Tile tile in tiles)
            {
                tile.Draw(spriteBatch);
#if Debug
                if (Game1.PlayerTileLocation.Intersects(tile.Bounds))
                    collisionColor = Color.Red;                
                else 
                    collisionColor = Color.Green;
                                
                Utility.RectClass.DrawBorder(Game1.pixel, spriteBatch, tile.Bounds, 1, Color.Yellow, collisionColor * .1f);
#endif               
            }        
        }
        
    }
}
