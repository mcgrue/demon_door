using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FsWorld = FarseerPhysics.Dynamics.World;

namespace DemonDoor
{
    class Corpse : ICollidable
    {
        public Corpse(World w, Vector2 r0)
        {
            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Dynamic;
            _fsBody.Position = r0;

            CircleShape shape = new CircleShape(2f, 1.0f);
            
            _fsFixture = _fsBody.CreateFixture(shape, this);
            _fsFixture.Restitution = 0.5f;
            _fsFixture.OnCollision += BehaviorCollided;
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

            // CB: oh god this line of code is terrible, figure out how to fix it
            return (other.UserData is World);
        }

        public void Collided(ICollidable other)
        {
        }

        public Vector2 Position
        {
            get
            {
                return _fsBody.Position;
            }
        }

        public float Theta
        {
            get
            {
                return _fsBody.Rotation;
            }
        }

        private Body _fsBody;
        private Fixture _fsFixture;

    }
}
