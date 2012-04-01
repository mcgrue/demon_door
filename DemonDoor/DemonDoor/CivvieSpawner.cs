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
        private Vector2 location;
        private SpriteBasis[] civvieSpriteBasisList;
        private int spawnRateFuzzMillis;

        public CivvieSpawner(World world, McgLayer layer, Vector2 location, TimeSpan spawnRate, SpriteBasis[] civvieSpriteBasis, int spawnRateFuzzMillis = 0)
        {
            this.layer = layer;
            this.world = world;
            this.spawnRate = spawnRate;
            this.location = location;
            this.civvieSpriteBasisList = civvieSpriteBasis;
            this.spawnRateFuzzMillis = spawnRateFuzzMillis;
        }
        
        public int GetX()
        {
            return (int)location.X;
        }

        public int GetY()
        {
            return (int)location.Y;
        }

        public RenderDelegate GetDrawDelegate()
        {
            return (x, y) => { };
        }

        private static int nextIdx = 0;
        public int getnextCivvieIdx() {
            nextIdx++;
            if( nextIdx >= civvieSpriteBasisList.Length ) {
                nextIdx = 0;
            }
            return nextIdx;
        }

        public void ProcessBehavior(Microsoft.Xna.Framework.GameTime time)
        {
            spawnTimeAccumulator += time.ElapsedGameTime;
            if (spawnTimeAccumulator > spawnRate)
            {
                layer.AddNode(new McgNode(new CivvieController(world, new Vector2(location.X, location.Y), new CivvieSprite(civvieSpriteBasisList[getnextCivvieIdx()])), layer, 
                    (int)location.X, (int)location.Y));
                spawnTimeAccumulator = TimeSpan.FromMilliseconds(VERGEGame.rand.Next(-spawnRateFuzzMillis, 0));
            }
        }
    }
}
