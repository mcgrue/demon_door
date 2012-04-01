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
        private CivvieSprite civvieSprite;

        public CivvieSpawner(World world, McgLayer layer, Point location, TimeSpan spawnRate, CivvieSprite civvieSprite)
        {
            this.layer = layer;
            this.world = world;
            this.spawnRate = spawnRate;
            this.location = location;
            this.civvieSprite = civvieSprite;
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
                layer.AddNode(new McgNode(new CivvieController(world, new Vector2(location.X, location.Y), civvieSprite), layer, location.X, location.Y));
                spawnTimeAccumulator = TimeSpan.Zero;
            }
        }
    }
}
