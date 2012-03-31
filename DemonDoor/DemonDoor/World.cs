using System;
using Microsoft.Xna.Framework;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FsWorld = FarseerPhysics.Dynamics.World;

namespace DemonDoor
{
    class World
    {
        public World(Vector2 gravity)
        {
            // set up the world.
            _fsWorld = new FsWorld(gravity);

            // set up the ground.
            _ground = new Body(_fsWorld);
            _ground.BodyType = BodyType.Static;

            PolygonShape groundShape = new PolygonShape(0.0f);
            groundShape.SetAsBox(1000f, 10f);

            Fixture f = _ground.CreateFixture(groundShape);
        }

        public void Simulate(GameTime now)
        {
            TimeSpan step = now.TotalGameTime - _last;

            _fsWorld.Step((float)step.TotalSeconds);

            _last = now.TotalGameTime;
        }

        internal Body NewBody()
        {
            return new Body(_fsWorld);
        }

        private FsWorld _fsWorld;
        private Body _ground;
        private TimeSpan _last = TimeSpan.Zero;
    }
}
