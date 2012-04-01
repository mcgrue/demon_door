
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

using XNAVERGE;
using System;

namespace DemonDoor
{

    class CivvieController : IDrawableThing, ICollidable, IBrainyThing
    {
        CivvieSprite myCorpse = null;
        Vector2 screen;

        private Body _fsBody;
        private Fixture _fsFixture;
        private World _world;

        private enum BehaviorState
        {
            Flying, Walking
        }
        private BehaviorState behaviorState;

        public CivvieController( World w, Vector2 r0, CivvieSprite sprite )
        {
            _world = w;

            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Dynamic;
            _fsBody.Position = r0;

            CircleShape shape = new CircleShape(1f, 1.0f);
            
            Fixture f = _fsBody.CreateFixture(shape, this);
            f.Restitution = 0.5f;

            myCorpse = sprite;
        
            _fsFixture = _fsBody.CreateFixture(shape, this);
            _fsFixture.Restitution = 0.5f;
            _fsFixture.OnCollision += BehaviorCollided;
        }

        public int GetX() {
            return (int)_fsBody.Position.X; 
        }
        public int GetY() {
            return (int)_fsBody.Position.Y;
        }

        RenderDelegate _myDrawDelegate;
        
        public RenderDelegate GetDrawDelegate() {
            if( _myDrawDelegate != null ) return _myDrawDelegate;

            _myDrawDelegate = ( int x, int y ) => {

                this.screen = Coords.Physics2Screen( new Vector2 { X = Position.X, Y = Position.Y } );

                // maybe update the screen here?

                myCorpse.Sprite.x = (int)screen.X - 8;
                myCorpse.Sprite.y = (int)screen.Y - 8;
                myCorpse.Sprite.Draw();
            };

            return _myDrawDelegate;
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
            return true;
        }

        public void Collided(ICollidable other)
        {
            if (other == _world)
            {
                Console.WriteLine("VELOCITY OF COLLISION: " + _fsBody.LinearVelocity.Y);
                if (Math.Abs(_fsBody.LinearVelocity.Y) < 1)
                {
                    this.behaviorState = BehaviorState.Walking;
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
            if (behaviorState == BehaviorState.Walking)
            {
                _fsBody.LinearVelocity = new Vector2(-20, _fsBody.LinearVelocity.Y);
            }
        }
    }
}
