using System;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using FsWorld = FarseerPhysics.Dynamics.World;

namespace DemonDoor
{
    class World : ICollidable
    {
        public World(Vector2 gravity, float floorY)
        {
            // set up the world.
            _fsWorld = new FsWorld(gravity);

            // set up the ground.
            _ground = new Body(_fsWorld);
            _ground.BodyType = BodyType.Static;
            _ground.Position = new Vector2 { X = 0.0f, Y = floorY - 10.0f };

            PolygonShape groundShape = new PolygonShape(0.0f);
            groundShape.SetAsBox(1000f, 10f);

            _fsFixture = _ground.CreateFixture(groundShape, this);
            _fsFixture.OnCollision += this.BehaviorCollided;
            _fsFixture.Friction = 1.0f;
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


        private bool BehaviorCollided(Fixture f1, Fixture f2, Contact contact)
        {
            Fixture self = null, other = null;

            if (f1 == _fsFixture)
            {
                self = f1;
                other = f2;
            }
            else if (f2 == _fsFixture)
            {
                self = f2;
                other = f1;
            }

            if (other.UserData is ICollidable)
            {
                this.Collided(other.UserData as ICollidable);
                (other.UserData as ICollidable).Collided(this);
            }

            return true;
        }

        public void Collided(ICollidable other)
        {
        }

        private FsWorld _fsWorld;
        
        private Body _ground;
        private Fixture _fsFixture;

        private TimeSpan _last = TimeSpan.Zero;
    }
}
