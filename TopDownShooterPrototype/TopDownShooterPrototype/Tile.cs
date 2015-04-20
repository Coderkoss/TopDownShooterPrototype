//#define Debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TopDownShooterPrototype
{
    public class Tile
    {        
        //TODO:Look into making tiles load in dynamiclly exp. when you take a spritesheet in it will load the tile size even if its not 64 maybe its 32 or 80?
        const int WIDTH = 64;
        const int HEIGHT = 64;
        public const int TILESIZE = 64;
        const int NUMBEROFTILES = 10;

        public Texture2D texture;
        public Vector2 position { get; set; }
        public Rectangle? sourceRectangle { get; set; }
        public Rectangle hitbox;
        public Rectangle widthHitbox;
        public Color color { get; set; }        
        List<Rectangle> tileSrcRects;
        int index;

        public Rectangle Bounds 
        {
            get { return new Rectangle((int)this.position.X,(int)this.position.Y,TILESIZE,TILESIZE);}
        }
        public Rectangle HitBox 
        {
            get { return hitbox; }
            set { hitbox = value; }
        }
        public Rectangle WidthHitbox 
        {
            get { return hitbox; }
            set { hitbox = value; }
        }
        public int Index 
        {
            get { return index; }
            set { index = value; }
        }

        public Tile(Vector2  position,int index,Color color)
        {
            texture = Game1.TileSheet;
            this.position = position;
            this.sourceRectangle = null;
            this.color = color;
            this.index = index;
            tileSrcRects = new List<Rectangle>();
            switch (index)
            {
                case 0:
                    hitbox = new Rectangle((int)this.position.X, (int)this.position.Y , TILESIZE , (int)(TILESIZE / 1.5f) );
                    this.color = Color.White;
                    break;
                case 1:
                    hitbox = new Rectangle((int)this.position.X + 22, (int)this.position.Y, TILESIZE /2 + 12, TILESIZE);
                    this.color = Color.White;
                    break;
                case 2:
                    hitbox = new Rectangle((int)this.position.X, (int)this.position.Y , TILESIZE - 22, TILESIZE);
                    this.color = Color.White;
                    break;
                case 3:
                    hitbox = new Rectangle((int)this.position.X, (int)this.position.Y + 45 -2, TILESIZE, TILESIZE /3);
                    this.color = Color.White;
                    break;
                case 4:
                    //No hitbox
                    this.color = Color.White;
                    break;
                case 5:
                    hitbox = new Rectangle((int)this.position.X,(int)this.position.Y, TILESIZE,(int)(TILESIZE / 1.5f) );
                    widthHitbox = new Rectangle((int)this.position.X  + 22,(int)this.position.Y ,(int)(TILESIZE / 1.5f),TILESIZE);
                    this.color = Color.White;
                    break;
                case 6:
                    hitbox = new Rectangle((int)this.position.X, (int)this.position.Y, TILESIZE, (int)(TILESIZE / 1.5f));
                    widthHitbox = new Rectangle((int)this.position.X,(int)this.position.Y,(int)(TILESIZE) - 22,TILESIZE);
                    this.color = Color.White;
                    break;
                case 7:
                    hitbox = new Rectangle((int)this.position.X, (int)this.position.Y + 45 - 2, TILESIZE, TILESIZE / 3);
                    widthHitbox = new Rectangle((int)this.position.X + 22, (int)this.position.Y, TILESIZE / 2 + 12, TILESIZE);
                    this.color = Color.White;
                    break;
                case 8:
                    hitbox = new Rectangle((int)this.position.X, (int)this.position.Y + 45 - 2, TILESIZE, TILESIZE / 3);
                    widthHitbox = new Rectangle((int)this.position.X, (int)this.position.Y, TILESIZE - 22, TILESIZE);
                    this.color = Color.White;
                    break;
                case 9:
                    hitbox = new Rectangle((int)this.position.X, (int)this.position.Y, TILESIZE , TILESIZE);
                    this.color = Color.White;
                    break;
                default:
                    break;
            }
            for (int i = 0; i < NUMBEROFTILES; i++)
            {
                try
                {
                    tileSrcRects.Add(new Rectangle(i * TILESIZE, 0, TILESIZE, TILESIZE));
                }
                catch (Exception)
                {
                    Console.WriteLine("Make sure if you add more tiles that you take change the number of tiles.");
                }
            }
            this.sourceRectangle = tileSrcRects[index];
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(Game1.TileSheet, position, sourceRectangle, color);
#if Debug
            Utility.RectClass.DrawBorder(Game1.pixel, spriteBatch, hitbox, 1, Color.Red, Color.Black * 0.5f);
            Utility.RectClass.DrawBorder(Game1.pixel, spriteBatch, widthHitbox, 1, Color.Blue, Color.Black * 0.5f);
#endif
        }
        
    }
}
