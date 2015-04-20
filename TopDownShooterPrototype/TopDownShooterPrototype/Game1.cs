#define Debug
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TopDownShooterPrototype
{
  
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public const int SCREEN_WIDTH = 1024;
        public const int SCREEN_HEIGHT = 768;
        public const float SCALE = 1f;
        public const float SOUND_VOLUME = .20f;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Random random;
        public static Rectangle ScreenBounds;
        Nazi nazi;        
        //Player feilds
        public static Rectangle PlayerTileLocation;
        public static Vector2 playerPosition;
        public static Texture2D TileSheet;
        public static Texture2D BulletSplashSpriteSheet;
        public TileMap[] chunk;
        public TileMap currentTileMap;
        Camera camera;
        //TODO: we need to clamp the camera to the tilemapsize and the 0 position on the x and y.
        //TODO: study how to stop the fact that if you move diagnal you move faster than when you move up and down
#if Debug
        SpriteFont debugFont;
        public static Texture2D pixel;   
#endif
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
             Content.RootDirectory = "Content";
        }
       
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.IsFullScreen = false;

            graphics.ApplyChanges();
            Window.Title = "Top Down Shooter Prototype";
            ScreenBounds = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
            random = new Random();
                       
            base.Initialize();
        }       
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
            nazi = new Nazi(Content);
            camera = new Camera(GraphicsDevice.Viewport);
            TileSheet = Content.Load<Texture2D>("TileMap");
            debugFont = Content.Load<SpriteFont>(@"Debug");
            BulletSplashSpriteSheet = Content.Load<Texture2D>(@"BulletSplashSpriteSheet");
            currentTileMap =new TileMap( CreateNewTileMap());
        }
        protected override void UnloadContent()
        {
           
        }      
        protected override void Update(GameTime gameTime)
        {
            Utility.Input.Update(); 
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
           
            nazi.Update(gameTime);
            playerPosition = nazi.Position;
            PlayerTileLocation = nazi.FeetCollisionRect;                      
            nazi.CheckTileCollision(currentTileMap);
            if(PlayerTileLocation.Y > camera.Position.Y + camera.View.Bounds.Bottom)
            {                
                camera.Position = new Vector3(camera.Position.X, camera.Position.Y + camera.View.Height, 0);
                //float ypos = MathHelper.Lerp(camera.Position.Y + camera.View.Height, camera.View.Height + camera.View.Height, .04f);
                //camera.Position = new Vector3(camera.Position.X, ypos + camera.View.Height, 0);
                //camera.Position = new Vector3(camera.Position.X, ypos, 0);

            }
            else if(PlayerTileLocation.Y < camera.Position.Y )
            {
                camera.Position = new Vector3(camera.Position.X, camera.Position.Y - camera.View.Height, 0);
            }
            else if (PlayerTileLocation.X > camera.Position.X + camera.View.Bounds.Right)
            {
                camera.Position = new Vector3(camera.Position.X + camera.View.Width, camera.Position.Y , 0);
            }
            else if (PlayerTileLocation.X < camera.Position.X)
            {
                camera.Position = new Vector3(camera.Position.X - camera.View.Width, camera.Position.Y , 0);
            }
            
                
            camera.Update(gameTime);
            base.Update(gameTime);
        } 
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
            
            currentTileMap.TileMapDraw(spriteBatch);                  
            nazi.Draw(spriteBatch);            
            spriteBatch.End();
            base.Draw(gameTime);
        }
        private int [,] CreateNewTileMap() 
        {
           
            return TileMap.Load(@"C:\Users\kevin\Documents\Visual Studio 2010\Projects\TopDownShooterPrototype\TopDownShooterPrototype\TopDownShooterPrototypeContent\TextFile1.txt");
        }
        
        private void UpdateChunks() 
        {      
            nazi.ResetNaziObjects();
        }
            
        public static float NextFloat(Random rand, float min, float max) 
        {
            if (max < min)
                throw new ArgumentException("max cannot be less than min");
            return (float)rand.NextDouble() * (max - min) + min;
        }
    }
}
