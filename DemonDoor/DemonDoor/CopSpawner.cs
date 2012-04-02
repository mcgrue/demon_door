using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAVERGE;
using Microsoft.Xna.Framework;

namespace DemonDoor
{
    class CopSpawner : IDrawableThing, IBrainyThing
    {
        TimeSpan spawnRate;
        private TimeSpan spawnTimeAccumulator;
        private McgLayer layer;
        private World world;
        private Vector2 location;
        private SpriteBasis copSpriteBasis;
        private int spawnRateFuzzMillis;
        private SpriteBasis bulletSpriteBasis;
        private Vector2 playerPosition;

        public CopSpawner(World world, McgLayer layer, Vector2 location, TimeSpan spawnRate, SpriteBasis copSpriteBasis, SpriteBasis bulletSpriteBasis, Vector2 playerPosition, int spawnRateFuzzMillis = 0)
        {
            this.layer = layer;
            this.world = world;
            this.spawnRate = spawnRate;
            this.location = location;
            this.copSpriteBasis = copSpriteBasis;
            this.spawnRateFuzzMillis = spawnRateFuzzMillis;
            this.playerPosition = playerPosition;
            this.bulletSpriteBasis = bulletSpriteBasis;
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

        public void ProcessBehavior(Microsoft.Xna.Framework.GameTime time)
        {
            spawnTimeAccumulator += time.ElapsedGameTime;
            if (spawnTimeAccumulator > spawnRate)
            {
                layer.AddNode(new McgNode(new CopController(world, new Vector2(location.X, location.Y), new CopSprite(copSpriteBasis), layer, bulletSpriteBasis, playerPosition), layer, 
                    (int)location.X, (int)location.Y));
                spawnTimeAccumulator = TimeSpan.FromMilliseconds(VERGEGame.rand.Next(-spawnRateFuzzMillis, 0));
            }
        }
    }
}
