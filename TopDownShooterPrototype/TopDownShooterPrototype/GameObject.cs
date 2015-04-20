using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TopDownShooterPrototype
{
    class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position;        
        protected int width, height;
        protected Rectangle sourceRectangle;
        protected Color color;
        protected float rotation;
        protected float scale;
        protected Vector2 orgin;
        protected static List<GameObject> gameObjects;

        public Texture2D Texture 
        {
            get { return texture; }
            set { texture = value; }            
        }
        public Vector2 Position 
        {
            get { return position; }
            set { position = value; }
        }
        public int Width 
        {
            get { return width; }
            set { width = value; }
        }
        public int Height 
        {
            get { return height; }
            set { height = value; }
        }
        public Rectangle SourceRectangle 
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }
        public Color Color 
        {
            get { return color; }
            set { color = value; }
        }
        public float Rotation 
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public float Scale 
        {
            get { return scale; }
            set { scale = value; }
        }        
        public Vector2 Orgin 
        {
            get { return orgin; }
            set { orgin = value; }
        }

        static GameObject() 
        {
            gameObjects = new List<GameObject>();
        }
        public GameObject(ContentManager content) 
        {
            this.texture = null;
            this.position = Vector2.Zero;
            this.color = Color.White;
            this.width = 80;
            this.height = 80;
            this.rotation = 0f;
            this.orgin = Vector2.Zero;
            this.sourceRectangle = new Rectangle(0, 0, width, height);
        }
        public virtual void Update(GameTime gametime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
