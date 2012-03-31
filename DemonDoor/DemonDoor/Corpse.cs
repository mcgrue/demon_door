using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FsWorld = FarseerPhysics.Dynamics.World;

namespace DemonDoor
{
    class Corpse
    {
        public Corpse(World w, Vector2 r0)
        {
            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Dynamic;
            _fsBody.Position = r0;
            _fsBody.Restitution = 0.5f;

            CircleShape shape = new CircleShape(2f, 1.0f);

            Fixture f = _fsBody.CreateFixture(shape);
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
    }
}
