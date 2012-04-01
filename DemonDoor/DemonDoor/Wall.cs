using System;
using System.Diagnostics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision;

namespace DemonDoor
{
    class Wall : ICollidable
    {
        public Wall(World w, float x, float sgn)
        {
            _world = w;
            Stickiness = 70;
            UpwardSpeedLimit = 5.0f;

            Vector2 center = new Vector2 { X = x - (10.0f * sgn), Y = 500.0f };

            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Static;
            _fsBody.Position = center;

            PolygonShape wallShape = new PolygonShape(0);
            wallShape.SetAsBox(10f, 1000f);

            _fsFixture = _fsBody.CreateFixture(wallShape, this);
            _fsFixture.OnCollision += BehaviorCollided;
            _fsFixture.AfterCollision += PhysicsPostSolve;
            _fsFixture.IsSensor = true;
        }

        public Wall(World w, Vector2[] vertices)
        {
            _world = w;
            Vertices vs = new Vertices(vertices);

            Stickiness = 70;
            UpwardSpeedLimit = 5.0f;

            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Static;

            PolygonShape wallShape = new PolygonShape(vs, 0);

            _fsFixture = _fsBody.CreateFixture(wallShape, this);
            _fsFixture.OnCollision += BehaviorCollided;
            _fsFixture.AfterCollision += PhysicsPostSolve;
        }

        public float Stickiness { get; set; }
        public float UpwardSpeedLimit { get; set; }
        
        private void PhysicsPostSolve(Fixture f1, Fixture f2, Contact contact)
        {
            Fixture self = null, other = null;

            Vector2 normal;
            FixedArray2<Vector2> points;

            contact.GetWorldManifold(out normal, out points);

            if (contact.FixtureA == _fsFixture)
            {
                self = contact.FixtureA;
                other = contact.FixtureB;
            }
            else if (contact.FixtureB == _fsFixture)
            {
                self = contact.FixtureB;
                other = contact.FixtureA;
            }

            // sticky behavior: null out all velocity normal to the surface of the wall, 
            // apply some shitty fake friction to the velocity parallel to the surface of the wall.
            Vector2 normalComponent, parallelComponent;

            normalComponent = normal * Vector2.Dot(normal, other.Body.LinearVelocity);
            parallelComponent = other.Body.LinearVelocity - normalComponent;

            float velMultiplier = 1f - Stickiness * (1 / 100000f);
            Vector2 newVelocity;

            if (other.Body.LinearVelocity.Length() < 1e-10 || parallelComponent.Length() < 1e-10)
            {
                newVelocity = Vector2.Zero;
            }
            else if (other.Body.LinearVelocity.Y > 0)
            {
                newVelocity = parallelComponent * Math.Min(parallelComponent.Length(), UpwardSpeedLimit) / parallelComponent.Length();
            }
            else
            {
                newVelocity = parallelComponent * velMultiplier;
            }

            other.Body.LinearVelocity = newVelocity;
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

        private World _world;
        private Body _fsBody;
        private Fixture _fsFixture;

    }
}
