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
    class Gun
    {
        public Gun(World w, Vector2 r, Vector2 size)
        {
            _fsBody = w.NewBody();
            _fsBody.Position = r;

            PolygonShape shape = new PolygonShape(1.0f);
            shape.SetAsBox(size.X, size.Y);

            _fsFixture = _fsBody.CreateFixture(shape);
            _fsFixture.IsSensor = true;

            _fsFixture.OnCollision += Shoot;
        }

        private bool Shoot(Fixture f1, Fixture f2, Contact contact)
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

            if (other == null)
            {
                Console.WriteLine("other is null!");
            }
            if (other.UserData is Corpse)
            {
                Corpse c = other.UserData as Corpse;
                Console.WriteLine("collided with corpse {0}, kickin' it", c);

                other.Body.ApplyLinearImpulse(Impulse);
            }

            return false;
        }

        public Vector2 Impulse { get; set; }

        private Fixture _fsFixture;
        private Body _fsBody;
    }
}
