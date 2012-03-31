
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace DemonDoor
{
    class Wall : ICollidable
    {
        public Wall(World w, float x, float sgn)
        {
            Stickiness = 70;
            StickinessRatchet = new Vector2 { X = 0, Y = -1 };
            StickinessRatchetBackspin = 5.0f;

            Vector2 center = new Vector2 { X = x - (10.0f * sgn), Y = 500.0f };

            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Static;
            _fsBody.Position = center;

            PolygonShape wallShape = new PolygonShape(0);
            wallShape.SetAsBox(10f, 1000f);

            _fsFixture = _fsBody.CreateFixture(wallShape, this);
            _fsFixture.OnCollision += PhysicsCollided;
            _fsFixture.OnCollision += BehaviorCollided;
        }

        public Wall(World w, Vector2[] vertices)
        {

        }

        public float Stickiness { get; set; }
        public Vector2? StickinessRatchet { get; set; }
        public float StickinessRatchetBackspin { get; set; }

        private bool PhysicsCollided(Fixture f1, Fixture f2, Contact contact)
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

            if (other.UserData is Corpse)
            {
                Corpse c = other.UserData as Corpse;
                //Console.WriteLine("collided with corpse {0}, stickin' it", c);

                // sticky behavior: null out all velocity normal to the surface of the wall, 
                // apply some shitty fake friction to the velocity parallel to the surface of the wall.
                float velMultiplier = 1f - Stickiness * (1 / 100000f);

                if (StickinessRatchet.HasValue && (Vector2.Dot(other.Body.LinearVelocity, StickinessRatchet.Value) < -StickinessRatchetBackspin))
                {
                    other.Body.LinearVelocity = new Vector2 { X = 0, Y = StickinessRatchetBackspin };
                }
                else
                {
                    other.Body.LinearVelocity = new Vector2 { X = 0, Y = other.Body.LinearVelocity.Y * velMultiplier };
                }
            }

            return false;
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

            return false;
        }

        public void Collided(ICollidable other)
        {
        }

        private Body _fsBody;
        private Fixture _fsFixture;

    }
}
