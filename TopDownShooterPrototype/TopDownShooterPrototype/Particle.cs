using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TopDownShooterPrototype
{
    class Particle
    {
        float lifeSpan,scale,lifeTimer,speed;       
        Vector2 velocity;
        Texture2D texture;
        Vector2 position;        
        int width, height,distance;
        Color color;                     
        bool isAlive;        
        Vector2 startPosition;
        public float LifeSpan
        {
            get { return lifeSpan; }
        }
        public Vector2 Velocity 
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public Texture2D Texture 
        {
            get { return texture; }
        }
        public Vector2 Position 
        {
            get { return position; }
            set { position = value; }
        }
        public float Speed 
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Width 
        {
            get { return width; }
            set { width = value; }
        }
        public int Height 
        {
            get { return height; }
            set { height = value;    }
        }
        public Color Color 
        {
            get { return color; }
            set { color = value;  }
        }
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value;   }
        }
        public float Scale 
        {
            get { return scale; }
            set { scale = value; }
        }
        public float LifeTimer
        {
            get { return lifeTimer; }
            set { lifeTimer = value;  }
        }
        public int Distance 
        {
            get { return distance; }
            set { distance = value; }
        }
        public Vector2 StartPosition 
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        public Particle(float lifeSpan, float scale, Color tint,Vector2 position,int distance,int speed) 
        {
            this.texture = Game1.pixel;
            this.position = position;
            this.width = Game1.pixel.Width;
            this.height = Game1.pixel.Height;
            this.speed = speed;
            this.color = tint;
            this.lifeSpan = lifeSpan;
            this.velocity = Vector2.Zero;
            this.isAlive = true;
            this.scale = scale;
            this.distance = distance;
            this.startPosition = this.position;
        }
        public void Update(GameTime gametime)
        {
            lifeSpan -= (float)gametime.ElapsedGameTime.TotalSeconds;
            if (lifeSpan < 0) 
            {
                isAlive = false;
            }
            this.position += velocity * speed * (float)gametime.ElapsedGameTime.TotalMilliseconds;        
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                spriteBatch.Draw(this.texture, this.position, new Rectangle(0, 0, width, height), this.color, 0, new Vector2(width / 2, height / 2), this.scale, SpriteEffects.None, 0f);
                
            }
        }
        
    }
}
