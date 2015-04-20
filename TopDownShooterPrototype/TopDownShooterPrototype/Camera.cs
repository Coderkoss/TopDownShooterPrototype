using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterPrototype
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        Vector3 position;
        float zoom;
        public Vector3 Position 
        {
            get { return position; }
            set { position = value; }
        }
        public Viewport View 
        {
            get { return view; }
            set { view = value; }
        }
        public Rectangle Bounds 
        {
            get { return new Rectangle((int)this.position.X, (int)this.position.Y, view.Width, view.Height); }
        }
            
        public Camera(Viewport View) 
        {
            this.view = View;
            this.zoom = 1.0f;
            this.position = new Vector3(0, 0, 0);
        }
        public void Update(GameTime gametime) 
        {
            if (Utility.Input.KeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                this.zoom += .1f;
            if (Utility.Input.KeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                this.zoom -= .1f;

            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(-position) * Matrix.CreateScale(zoom);
        }
                       
    }
}
