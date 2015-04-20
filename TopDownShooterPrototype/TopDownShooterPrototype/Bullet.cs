#define Debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TopDownShooterPrototype
{
    class Bullet:GameObject
    {
        float speed;
        public Vector2 direction;       
        public Rectangle HitBox 
        {            
            get
            {
                return new Rectangle((int)(position.X + (width / 2)  / 2) + (width / 4)  /2 , (int)(this.position.Y + (height / 2) /2) + (width / 4) / 2, width / 4,height / 4);
                                    
            }
        }        
        public float Speed 
        {
            get { return speed; }
            set { speed = value; }
        }
        SoundEffect singleShot;
        SoundEffectInstance singleShotInstance;
        public Nazi.Direction CurrentDirection;

        public Bullet(ContentManager Content):base(Content) 
        {
            this.texture = Content.Load<Texture2D>("ball");
            this.width = 32;
            this.height = 32;
            this.sourceRectangle = new Rectangle(0,0,width,height);
            this.speed = 10f;
            this.scale = Game1.SCALE;
            this.color = Color.White;
            
            this.direction = Vector2.Zero;
            singleShot = Content.Load<SoundEffect>("BulletSnd");
            singleShotInstance = singleShot.CreateInstance();
            singleShotInstance.Volume = Game1.SOUND_VOLUME ;
            singleShotInstance.Play();
        }
        public override void Update(GameTime gametime)
        {
            this.direction.Normalize();               
            this.position += direction * speed * (float)gametime.ElapsedGameTime.TotalMilliseconds/ 16;           
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position,
                             this.sourceRectangle, color,
                             rotation, orgin, scale, SpriteEffects.None, 0);
#if Debug
            Utility.RectClass.DrawBorder(Game1.pixel, spriteBatch, this.HitBox, 1, Color.Red, Color.Gray * 0.5f);
#endif
        }
        public void SetDirection(Nazi.Direction Direction) 
        {
            CurrentDirection = Direction;
           
        }
        
            
    }
}
