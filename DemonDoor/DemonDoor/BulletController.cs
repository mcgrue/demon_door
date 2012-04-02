
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

using XNAVERGE;
using System;

namespace DemonDoor
{

    class BulletController : IDrawableThing, ICollidable, IBrainyThing
    {
        BulletSprite bulletSprite = null;
        Vector2 screen;

        internal Body _fsBody;
        private Shape _fsShape;
        private Fixture _fsFixture;
        private World _world;

        public BulletController(World w, Vector2 r0, BulletSprite sprite, Vector2 vectorToPlayer)
        {
            _world = w;

            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Dynamic;
            _fsBody.Position = r0;

            bulletSprite = sprite;

            vectorToPlayer.Normalize();
            this.vectorToPlayer = vectorToPlayer;

            MakeLivingFixture();
        }

        private void MakeLivingFixture()
        {
            if (_fsFixture != null)
            {
                _fsBody.DestroyFixture(_fsFixture);
            }

            _fsShape = new CircleShape(1f, 1.0f);
            _fsFixture = _fsBody.CreateFixture(_fsShape, this);
            _fsFixture.Restitution = 0.2f;
            _fsFixture.OnCollision += BehaviorCollided;
        }

        public int GetX()
        {
            return (int)_fsBody.Position.X;
        }
        public int GetY()
        {
            return (int)_fsBody.Position.Y;
        }

        RenderDelegate _myDrawDelegate;
        private Vector2 vectorToPlayer;

        public RenderDelegate GetDrawDelegate()
        {
            if (_myDrawDelegate != null) return _myDrawDelegate;

            _myDrawDelegate = (int x, int y) =>
            {

                this.screen = Coords.Physics2Screen(new Vector2 { X = Position.X, Y = Position.Y });

                // maybe update the screen here?

                bulletSprite.Sprite.x = (int)screen.X - 8;
                bulletSprite.Sprite.y = (int)screen.Y - 8;
                bulletSprite.Sprite.Draw();
            };

            return _myDrawDelegate;
        }

        private bool BehaviorCollided(Fixture f1, Fixture f2, Contact contact)
        {
            return false;
        }

        public void Collided(ICollidable other)
        {
            if (other == _world)
            {


                if (other is CivvieController)
                {
                    var otherCivvie = other as CivvieController;
                    otherCivvie.Die();
                }
            }
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

        public void ProcessBehavior(GameTime time)
        {
            _fsBody.LinearVelocity = vectorToPlayer * 75;
        }
    }
}
