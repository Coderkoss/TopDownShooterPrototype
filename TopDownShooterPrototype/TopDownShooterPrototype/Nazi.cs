//#define debug
//TODO:study Resolution independency
//TODO: View all of Oyyou on youtube tutorials
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Utility;
namespace TopDownShooterPrototype
{
    class Nazi:Animation 
    {
        public enum Direction
        {
            Left,
            Right,
            Down,
            Up
        }
        const float MAXSPEED = Tile.TILESIZE / 45;        
        const float TIME_BETWEEN_SHOTS = 200;
        Vector2 velocity;
        List<Bullet> bullets;
        List<BulletSplash> bulletSplashes;
        int health;
        ContentManager content;
        Direction direction;        
        float shotTimer;
        SoundEffect hurtSnd;
        SoundEffect rockSnd;
        SoundEffect walkSnd;
        SoundEffectInstance hurtSound;
        SoundEffectInstance rockSound;
        SoundEffectInstance walkSound;
        bool mLeft, mRight, mUp, mDown;
        int leftOffset,rightOffset,topOffset;
        float acceleration;
        public bool IsAlive 
        {
            get 
            {
                if (health > 0)
                    return true;
                else 
                    return false;
            }
           
        }        
        public Rectangle CollisionRect
        {
            get 
            {
                return new Rectangle((int)this.position.X + leftOffset , (int)this.position.Y + topOffset, this.width - rightOffset , this.height - topOffset );
            }            
        }
        public Rectangle FeetCollisionRect 
        {
            get {return new Rectangle(CollisionRect.X + 15,CollisionRect.Y + 56,CollisionRect.Width/ 2,CollisionRect.Height/3); }
        }
        public List<Bullet> Bullets 
        {
            get { return bullets; }
        }
        public Direction CurDirection 
        {
            get { return direction; }
            set { direction = value; }
        }
        float angle;
        ParticleGenerate pg;

       
        public Nazi(ContentManager Content) : base(Content) 
        {
            this.texture = Content.Load<Texture2D>(@"topdownnazi");
            this.content = Content;
            this.bullets = new List<Bullet>();
            this.direction = Direction.Right;
            this.width = 150;
            this.height = 118; 
            this.Interval = 50;
            this.scale = Game1.SCALE;
            this.shotTimer = TIME_BETWEEN_SHOTS;
            this.hurtSnd = content.Load<SoundEffect>(@"Hurt");
            this.rockSnd = content.Load<SoundEffect>(@"RockSmash");
            this.walkSnd = content.Load<SoundEffect>(@"walk");
            this.hurtSound = hurtSnd.CreateInstance();
            this.rockSound = rockSnd.CreateInstance();
            this.walkSound = walkSnd.CreateInstance();
            this.walkSound.Volume = Game1.SOUND_VOLUME;
            this.position = new Vector2(60, 50);
            this.sourceRectangle = new Rectangle(0, 0, Width, Height);
            this.leftOffset = 40;
            this.rightOffset = 90;
            this.topOffset = 30;
            this.health = 100;
            this.acceleration = 0;
            this.mDown = false;
            this.mUp = false;
            this.mRight = false;
            this.mLeft = false;
            this.bulletSplashes = new List<BulletSplash>();
            this.pg = new ParticleGenerate(100, 200f,5f);
        }
        public override void Update(GameTime gametime)
        {
            GamePadState gs = GamePad.GetState(PlayerIndex.One);//create a game pad state
            velocity = new Vector2(gs.ThumbSticks.Left.X, -gs.ThumbSticks.Left.Y);// set the velocity to the thumb sticks
            HandleMoveInput(gametime);           
            shotTimer -= (int)gametime.ElapsedGameTime.TotalMilliseconds;// this is the timer that we use to tell how many shots between shots are fired
            UpdateBullet(gametime);
            UpdateBulletSplashes(gametime);
            UpdateMuzzleFlashes(gametime);
            HandleGunFire(gametime);//this works out what way the gun is facing and fires accordingly
            pg.Update(gametime);// this is the particle genorator update

            if (acceleration >= MAXSPEED)
            {
                acceleration = MAXSPEED;
            }
            
           
            AnimateNazi();      
           
            this.position += velocity  * (int)gametime.ElapsedGameTime.TotalMilliseconds / 16;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet item in bullets)
            {
                item.Draw(spriteBatch);
            }
            foreach (MuzzleFlash item in MuzzleFlash.muzzleFlashes)
            {
                item.Draw(spriteBatch);
            }
            foreach (BulletSplash item in bulletSplashes)
            {
                item.Draw(spriteBatch);
            }
            pg.Draw(spriteBatch);
            spriteBatch.Draw(this.texture, this.position, 
                             this.sourceRectangle, color,
                             rotation, orgin, scale, SpriteEffects.None, 0);
#if debug
          Utility.RectClass.DrawBorder(Game1.pixel,spriteBatch,CollisionRect,1,Color.AliceBlue,Color.Gray * 0.4f);
          Utility.RectClass.DrawBorder(Game1.pixel, spriteBatch, FeetCollisionRect, 1, Color.Red, Color.Orange * 0.6f);
#endif
        }
        public void AnimateNazi() 
        {
            switch (direction)
            {
                case Direction.Left:
                    this.SourceRectangle = new Rectangle(Width * curFrame, 236, width, height);                    
                    break;
                case Direction.Right:
                    this.SourceRectangle = new Rectangle(Width * curFrame,354, width, height);                    
                    break;
                case Direction.Down:
                    this.SourceRectangle = new Rectangle(Width * curFrame, 0, width, height);                       
                    break;
                case Direction.Up:
                    this.SourceRectangle = new Rectangle(Width * curFrame, 118, width, height);                                          
                    break;
                default:
                    break;
            }
        }
        public void FireGun(GameTime gameTime) 
        {
           Bullet tempBullet = new Bullet(content);
           tempBullet.Position = new Vector2(this.position.X + this.Width / 4,this.position.Y + this.height / 1.6f );

           switch (direction)
           {
               case Direction.Left:
                   tempBullet.direction = new Vector2(-1,0);
                   tempBullet.Position = new Vector2(this.position.X, this.position.Y + this.height / 1.6f);
                   //tempBullet.Speed += MAXSPEED;
                   tempBullet.SetDirection(Direction.Left);
                   MuzzleFlash.muzzleFlashes.Add(new MuzzleFlash(content,new Vector2(this.position.X + 6,this.position.Y + 80),Direction.Left));
                   break;
               case Direction.Right:
                   tempBullet.direction = new Vector2(1, 0);
                   tempBullet.Position = new Vector2(this.position.X + this.Width / 1.5f,this.position.Y + this.height / 1.6f);
                   //tempBullet.Speed += MAXSPEED;
                   tempBullet.SetDirection(Direction.Right);
                   MuzzleFlash.muzzleFlashes.Add(new MuzzleFlash(content,new Vector2(this.position.X + 98,this.position.Y + 82 ),Direction.Right));
                   break;
               case Direction.Down:
                   tempBullet.direction = new Vector2(0, 1);
                   tempBullet.SetDirection(Direction.Down);
                   //tempBullet.Speed += MAXSPEED;
                   MuzzleFlash.muzzleFlashes.Add(new MuzzleFlash(content,new Vector2(this.position.X + 45,this.position.Y+ 112),Direction.Down));
                   break;
               case Direction.Up:
                   tempBullet.direction = new Vector2(0,-1);
                   tempBullet.SetDirection(Direction.Up);
                   //tempBullet.Speed += MAXSPEED;
                  // MuzzleFlash.muzzleFlashes.Add(new MuzzleFlash(content, new Vector2(0, 0), Direction.Up));
                   break;
               default:
                   break;
           }
           bullets.Add(tempBullet);
                       
        }
        public void BulletToScreenCollCheck(Bullet bullet)
        {
            if (bullet.HitBox.Left < 0)
            {
                bullets.Remove(bullet);
            }
            else if (bullet.HitBox.Right > Game1.SCREEN_WIDTH + bullet.Width )
            {
                bullets.Remove(bullet);
            }
            else if (bullet.HitBox.Top < 0)
            {
                bullets.Remove(bullet);
            }
            else if (bullet.HitBox.Bottom > Game1.SCREEN_HEIGHT + bullet.Height)
            {
                bullets.Remove(bullet);
            }
        }
        public void TakeDamage() 
        {
            health -= 10;
            hurtSound.Play();
            this.position.X -= 14;
            if (!IsAlive) { this.color = Color.Red; }
        }
        public void HandleMoveInput(GameTime gametime) 
        {
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            angle = (float)Math.Atan2(position.Y,position.X);
            rotation = MathHelper.ToRadians(angle);
            int meter = Tile.TILESIZE /10;

            if (Input.KeyDown(Keys.Up) || Input.ButtonPress(Buttons.LeftThumbstickUp))
                mUp = true;
            else if (Input.KeyDown(Keys.Down) || Input.ButtonPress(Buttons.LeftThumbstickDown))
                mDown = true;
            else if (Input.KeyDown(Keys.Right)|| Input.ButtonPress(Buttons.LeftThumbstickRight))
                mRight = true;
            else if (Input.KeyDown(Keys.Left) || Input.ButtonPress(Buttons.LeftThumbstickLeft))
                mLeft = true;
            else
            {
                mUp = false;
                mDown = false;
                mRight = false;
                mLeft = false;
            }
            acceleration += meter;
            if(mUp)
            {
                velocity.Y -= acceleration;
            }
            else if(mDown)
            {
                velocity.Y += acceleration;
            }
            else if(mRight)
            {
                velocity.X += acceleration;
            }
            else if (mLeft)
            {
                velocity.X -= acceleration;
            }
            else 
            {
                velocity *= velocity * new Vector2(.1f, .1f);
            }
            
             
            

            
            //#region input
            //if(Input.KeyDown(Keys.Up)&& Input.KeyDown(Keys.Right))
            //{             
            //    walkSound.Play();
            //    direction = Direction.Up;
            //    acceleration += meter;
            //    velocity = new Vector2(1f, -.005f);
            //    if (timer >= interval) 
            //    {
            //        timer = 0;
            //        curFrame++;
            //        if (curFrame >= 7)
            //        { curFrame = 0; }
            //    }
            //}
            //else if (Input.KeyDown(Keys.Up) && Input.KeyDown(Keys.Left))
            //{
            //    walkSound.Play();
            //    direction = Direction.Up;
            //    acceleration += meter;
            //    velocity = new Vector2(-1f, -.005f);
            //    if (timer >= interval)
            //    {
            //        timer = 0;
            //        curFrame++;
            //        if (curFrame >= 7)
            //        { curFrame = 0; }
            //    }
            //}
            //else if (Input.KeyDown(Keys.Down)|| Input.ButtonPress(Buttons.DPadDown))
            //{
            //    walkSound.Play();
            //    direction = Direction.Down;
            //    acceleration += meter;
            //    velocity = new Vector2(0,1);
            //    if (timer >= interval)
            //    {
            //        timer = 0;
            //        curFrame++;
            //        if (curFrame >= 7)
            //        { curFrame = 0; }
            //    }
                
            //}            
            //else if (Keyboard.GetState().IsKeyDown(Keys.Up) || Input.ButtonPress(Buttons.DPadUp))
            //{
            //    walkSound.Play();
            //    direction = Direction.Up;
            //    acceleration += meter;

            //    velocity = new Vector2(0, -1);
            //    if (timer >= interval)
            //    {
            //        timer = 0;
            //        curFrame++;
            //        if (curFrame >= 7)
            //        { curFrame = 0; }
            //    }
               
            //}             
            //else if (Keyboard.GetState().IsKeyDown(Keys.Left) || Input.ButtonPress(Buttons.DPadLeft))
            //{
            //    walkSound.Play();
            //    direction = Direction.Left;
            //    acceleration += meter;

            //    velocity = new Vector2(-1,0);
            //    if (timer >= interval)
            //    {
            //        timer = 0;
            //        curFrame++;
            //        if (curFrame >= 7)
            //        { curFrame = 0; }

            //    }
               
            //}
            //else if (Input.KeyDown(Keys.Right) || Input.ButtonPress(Buttons.DPadRight))
            //{
                
            //    walkSound.Play();                
                
            //    direction = Direction.Right;
            //    acceleration += meter;

            //    velocity = new Vector2(1,0);
            //    if (timer >= interval)
            //    {
            //        timer = 0;
            //        curFrame++;
            //        if (curFrame >= 7)
            //        {
            //            curFrame = 0;
            //        }
            //    }
              
            //}
            //else if (Input.ButtonPress(Buttons.LeftTrigger) && Input.ButtonPress(Buttons.DPadUp)) 
            //{
            //    walkSound.Play();
            //    direction = Direction.Right;
            //    acceleration += meter;

            //    velocity = new Vector2(1, .7f);
            //    if (timer >= interval)
            //    {
            //        timer = 0;
            //        curFrame++;
            //        if (curFrame >= 7)
            //        {
            //            curFrame = 0;
            //        }
            //    }
            //}
            //else
            //{
            //    walkSound.Stop();
            //    acceleration = 0;
            //    //this.velocity -= velocity * new Vector2(3f, 3f);
            //    curFrame = 0;
            //}
            //#endregion
        }
      
        public void CheckTileCollision(TileMap tileMap) 
        {
            for (int i = 0; i < tileMap.Tiles.Count; i++)
            {
                if(this.FeetCollisionRect.Intersects(tileMap.Tiles[i].hitbox))
                {                   
                    int overlap;
                    switch (direction)
                    {                            
                        case Direction.Left:
                            overlap = tileMap.Tiles[i].hitbox.Right - this.FeetCollisionRect.Left;
                            this.velocity = Vector2.Zero;
                            this.position.X = this.position.X + overlap;
                            break;
                        case Direction.Right:
                            overlap = this.FeetCollisionRect.Right - tileMap.Tiles[i].hitbox.Left;
                            this.velocity = Vector2.Zero;
                            this.position.X = this.position.X - overlap;
                            break;
                        case Direction.Down:
                            overlap =  this.FeetCollisionRect.Bottom - tileMap.Tiles[i].hitbox.Top;
                            this.velocity = Vector2.Zero;
                            this.position.Y = this.position.Y - overlap;
                            break;
                        case Direction.Up:
                            overlap =  tileMap.Tiles[i].hitbox.Bottom - this.FeetCollisionRect.Top;
                            this.velocity = Vector2.Zero;
                            this.position.Y = this.position.Y + overlap;
                            break;
                        default:
                            break;
                    }                   
                }
            }
            CheckTileWidthCollision(tileMap);
            BulletTileCheckCollision(tileMap);
        }
        public void CheckTileWidthCollision(TileMap tileMap) 
        {
            for (int i = 0; i < tileMap.Tiles.Count; i++)
            {
                if (this.FeetCollisionRect.Intersects(tileMap.Tiles[i].widthHitbox))
                {
                    int overlap;
                    switch (direction)
                    {
                        case Direction.Left:
                            overlap = tileMap.Tiles[i].widthHitbox.Right - this.FeetCollisionRect.Left;
                            this.velocity = Vector2.Zero;
                            this.position.X = this.position.X + overlap;
                            break;
                        case Direction.Right:
                            overlap = this.FeetCollisionRect.Right - tileMap.Tiles[i].widthHitbox.Left;
                            this.velocity = Vector2.Zero;
                            this.position.X = this.position.X - overlap;
                            break;
                        case Direction.Down:
                            overlap = this.FeetCollisionRect.Bottom - tileMap.Tiles[i].widthHitbox.Top;
                            this.velocity = Vector2.Zero;
                            this.position.Y = this.position.Y - overlap;
                            break;
                        case Direction.Up:
                            overlap = tileMap.Tiles[i].widthHitbox.Bottom - this.FeetCollisionRect.Top;
                            this.velocity = Vector2.Zero;
                            this.position.Y = this.position.Y + overlap;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public void BulletTileCheckCollision(TileMap tileMap) 
        {
           for (int i = 0; i < tileMap.Tiles.Count; i++)
            {
                for (int y = 0; y < bullets.Count; y++)
                {
                    if (tileMap.Tiles[i].Index == 9) 
                    {
                         if (bullets[y].HitBox.Intersects(tileMap.Tiles[i].hitbox)) 
                         {
                             BulletSplash splash = new BulletSplash(content, bullets[y].CurrentDirection);
                             splash.Position = bullets[y].Position;                             
                             bulletSplashes.Add(splash);
                             bullets.Remove(bullets[y]);
                             y--;
                         }
                    }
                    
                }                 
            }
        }
        private void UpdateBullet(GameTime gametime) 
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gametime);
                BulletToScreenCollCheck(bullets[i]);
            }
        }
        private void UpdateMuzzleFlashes(GameTime gametime) 
        {
            for (int i = MuzzleFlash.muzzleFlashes.Count - 1; i >= 0; i--)
            {
                MuzzleFlash.muzzleFlashes[i].Update(gametime);
            }
        }
        private void UpdateBulletSplashes(GameTime gametime) 
        {
            for (int i = bulletSplashes.Count - 1; i >= 0; i--)
            {
                bulletSplashes[i].Update(gametime);
                if (bulletSplashes[i].Alive != true)
                {
                    bulletSplashes.Remove(bulletSplashes[i]);
                }
            }
        }
        private void HandleGunFire(GameTime gametime) 
        {
            if (Input.KeyDown(Keys.Space) || Input.ButtonPress(Buttons.A))
            {
                if (shotTimer <= 0)
                {
                    FireGun(gametime);
                    shotTimer = TIME_BETWEEN_SHOTS;
                }
            }   
        }
        public void ResetNaziObjects()
        {
            bullets.Clear();
            bulletSplashes.Clear();
        }
        public void GenerateBlood() 
        {
            pg = new ParticleGenerate(100, 60f,5f);
        }
        //TODO:put in a method that clears all list of projectiles/Particles
        //TODO:Fix the particle gen
        //TODO:set up some medthods for drawing game items
        //TODO: make sure you showcase the game on your blog
        //TODO:Implement the way you have decided to make levels        
        //TODO:create a Enemy Class put some enemys on the screen and get them moving around
        //TODO:make a bullet fire a bit more random  so sometimes when you shoot your player won't shoot straight
    }
}
