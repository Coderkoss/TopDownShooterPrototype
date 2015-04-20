using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TopDownShooterPrototype
{
    class BulletSplash:Animation
    {
        const int FRAMES = 9;
        bool Animating;
        bool alive;
        SoundEffect SplashSnd;
        SoundEffectInstance SplashSndInstance;

        public bool Alive 
        {
            get { return alive; }
            set { alive = value; }
        }
        public BulletSplash(ContentManager Content,Nazi.Direction direction):base(Content) 
        {
            
            this.timer = 0;
            this.interval = 30f;
            this.curFrame = 0;
            this.texture = Game1.BulletSplashSpriteSheet;
            this.color = Color.White;
            this.width = 32;
            this.height = 32;
            this.scale = Game1.SCALE;
            this.Animating = true;
            this.orgin = new Vector2(0, 0);            
            this.SplashSnd = Content.Load<SoundEffect>(@"BulletSplashSnd");
            this.sourceRectangle = new Rectangle(0, 0, width, height);
            alive = true;
            SplashSndInstance = SplashSnd.CreateInstance();
            SplashSndInstance.Volume = Game1.SOUND_VOLUME;
            SplashSndInstance.Play();

        }
        public override void Update(GameTime gametime)
        {
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            Animate(ref timer);
            Dispose();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            SourceRectangle = new Rectangle(curFrame * Width, 0, Width, Height);
            spriteBatch.Draw(this.texture, this.position,
                             this.sourceRectangle, color,
                             rotation, orgin, scale, SpriteEffects.None, 0);
        }
        private void Animate(ref float timer) 
        {
            if (Animating) 
            {
                if (timer > Interval) 
                {
                    curFrame++;
                    if (curFrame >= FRAMES) 
                    {
                        curFrame = FRAMES;
                        Animating = false;  
                    }
                    timer = 0;
                }            
            }
        }
        private void Dispose() 
        {
            if (Animating == false) 
            {
                alive = false;  
            }
        }
    }
}
