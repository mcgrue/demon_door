using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAVERGE;
using Microsoft.Xna.Framework;

namespace DemonDoor
{
    class CivvieSpawner : IDrawableThing, IBrainyThing
    {
        TimeSpan spawnRate;
        private TimeSpan spawnTimeAccumulator;
        private McgLayer layer;
        private World world;
        private Point location;
        private SpriteBasis civvieSpriteBasis;
        private int spawnRateFuzzMillis;

        public CivvieSpawner(World world, McgLayer layer, Point location, TimeSpan spawnRate, SpriteBasis civvieSpriteBasis, int spawnRateFuzzMillis = 0)
        {
            this.layer = layer;
            this.world = world;
            this.spawnRate = spawnRate;
            this.location = location;
            this.civvieSpriteBasis = civvieSpriteBasis;
            this.spawnRateFuzzMillis = spawnRateFuzzMillis;
        }
        
        public int GetX()
        {
            return location.X;
        }

        public int GetY()
        {
            return location.Y;
        }

        public RenderDelegate GetDrawDelegate()
        {
            return (x, y) => { };
        }

        public void ProcessBehavior(Microsoft.Xna.Framework.GameTime time)
        {
            spawnTimeAccumulator += time.ElapsedGameTime;
            if (spawnTimeAccumulator > spawnRate)
            {
                layer.AddNode(new McgNode(new CivvieController(world, new Vector2(location.X, location.Y), new CivvieSprite(civvieSpriteBasis)), layer, location.X, location.Y));
                spawnTimeAccumulator = TimeSpan.FromMilliseconds(VERGEGame.rand.Next(-spawnRateFuzzMillis, 0));
            }
        }
    }
}
