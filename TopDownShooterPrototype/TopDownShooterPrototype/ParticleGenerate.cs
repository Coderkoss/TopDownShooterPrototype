using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownShooterPrototype
{
    class ParticleGenerate
    {
        
        List <Particle> particles;
        Vector2 position;
        int sizeRange;          
        int numberOfParticles;
        float lifedurationRange;
        float particleSpawnDelay;
        float partilcleSpawnTimer;
        Color baseColor;
        float lifeDurationMax;
        public List<Particle> Particles
        {
            get { return particles; }
            set { particles = value; }
        }
        public Vector2 Position 
        {
            get { return position; }
            set { position = value; }
        }
        public int Size
        {
            get { return sizeRange; }
            set{sizeRange = value;  }
        }
        public int NumberOfParticles
        {
            get { return numberOfParticles; }
            set { numberOfParticles = value; }
        }
        public float Duration
        {
            get { return lifedurationRange; }
            set { lifedurationRange = value;  }
        }
        public float ParticleSpawnDelay
        {
            get { return particleSpawnDelay; }
            set { particleSpawnDelay = value;     }
        }
      
        public Color BaseColor 
        {
            get { return baseColor; }
            set { baseColor= value;     }
        } 
        
        public ParticleGenerate(int numberOfParticles,float particleSpawnDelay,float lifeDurationMax) 
        {
            //base
            this.particles = new List<Particle>();    
            this.position = new Vector2(Game1.SCREEN_HEIGHT / 2, Game1.SCREEN_WIDTH / 2);
            //base
            this.lifeDurationMax = lifeDurationMax;                 
            this.numberOfParticles = numberOfParticles;            
            this.particleSpawnDelay = particleSpawnDelay;
                            
        }
        
        public void Update(GameTime gameTime) 
        {
            partilcleSpawnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds; 
            for (int i = Particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(gameTime);
            }           
            bool numberOfParticlesCheck = particles.Count <= NumberOfParticles;
            if (partilcleSpawnTimer >= particleSpawnDelay ) 
            {
                    Emit();
                    partilcleSpawnTimer = 0;                 
            }                       
            
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            foreach (Particle item in Particles)
            {
                item.Draw(spriteBatch);
            }
        }    
        public void Emit() 
        {
            lifedurationRange = Game1.NextFloat(Game1.random, 0.01f, lifeDurationMax);
            Particle tempParticle = new Particle(lifedurationRange, 1, Color.Black, this.position, 2, 1);
            tempParticle.Scale = 5;           
            tempParticle.Velocity = new Vector2(-1, 0);
            tempParticle.Speed = .04f;
            Particles.Add(tempParticle);
        }


    }
}
