using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TopDownShooterPrototype
{
    class MuzzleFlash
    {
        public static List<MuzzleFlash> muzzleFlashes = new List<MuzzleFlash>();
        const float LIFETIMER =20.0F;
        public Texture2D texture { get; set; }
        public Vector2 position { get; set; }
        public float rotation { get; set; }        
        public Rectangle? sourceRectangle { get; set; }
        public Color color { get; set; }
        public Vector2 origin { get; set; }
        public Vector2 scale { get; set; }
        public SpriteEffects effects { get; set; }
        public float layerDepth { get; set; }
        float elaspedTime;
        public bool Alive;
        
        public MuzzleFlash(ContentManager content,Vector2 position,Nazi.Direction direction) 
        {
            Alive = true;
            this.texture = content.Load<Texture2D>("topdownnazi");
            this.position = position;
            this.rotation = 0.0f;
            this.color = Color.White;
            this.origin = Vector2.Zero;
            this.scale = new Vector2(1,1);

            this.effects = SpriteEffects.None;
            this.layerDepth = 0f;

            if(direction == Nazi.Direction.Down)
            {
                 this.sourceRectangle = new Rectangle(1556,246,20,28);
            }
            if(direction == Nazi.Direction.Left)
            {
                this.sourceRectangle = new Rectangle(1550,274,28,20);
            }
            if(direction == Nazi.Direction.Right)
            {
                this.sourceRectangle = new Rectangle(1550,302,28,20);
            }
            
            muzzleFlashes.Add(this);
        }
        public void Update(GameTime gametime) 
        {
             elaspedTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;
             if (LIFETIMER < elaspedTime) 
             {
                 Alive = false;
                 muzzleFlashes.Remove(this);
                 elaspedTime = 0.0f;
             }    
        }
        public void Draw(SpriteBatch spriteBatch) 
        {

            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }



        
    }
}
