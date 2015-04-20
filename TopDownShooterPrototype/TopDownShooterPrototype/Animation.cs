using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TopDownShooterPrototype
{
    class Animation:GameObject
    {
        protected float timer;
        protected float interval;
        protected int curFrame;        
        protected float Interval 
        {
            get { return interval; }
            set { interval = value;}
        }
        protected int CurrentFrame
        {
            get { return curFrame; }
            set { curFrame = value;}
        }

        public Animation(ContentManager Content):base(Content)
        {
            timer = 0;
            interval = 200f;
            curFrame = 0;           
            sourceRectangle = new Rectangle(0, 0, width, height);            
        }
    }
}
