
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

using XNAVERGE;
using System;

namespace DemonDoor
{

    class CopController : IDrawableThing, ICollidable, IBrainyThing
    {
        CopSprite copSprite = null;
        Vector2 screen;

        private Body _fsBody;
        private Shape _fsShape;
        private Fixture _fsFixture;
        private World _world;

        public enum BehaviorState
        {
            Flying, PatrollingLeft, PatrollingRight, Dead, Aiming, Shooting
        }
        public BehaviorState behaviorState = BehaviorState.Flying;

        public CopController( World w, Vector2 r0, CopSprite sprite )
        {
            _world = w;

            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Dynamic;
            _fsBody.Position = r0;
            
            copSprite = sprite;

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

        private void MakeDeadFixture()
        {
            if (_fsFixture != null)
            {
                _fsBody.DestroyFixture(_fsFixture);
            }

            PolygonShape shape = new PolygonShape(1.0f);
            shape.SetAsBox(1.0f, 1.0f);
            _fsFixture = _fsBody.CreateFixture(_fsShape, this);
            _fsFixture.Restitution = 0.2f;
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

                copSprite.Sprite.x = (int)screen.X - 8;
                copSprite.Sprite.y = (int)screen.Y - 8;
                copSprite.Sprite.Draw();
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

            if (behaviorState == BehaviorState.PatrollingLeft || behaviorState == BehaviorState.PatrollingRight)
            {
                if (other.UserData is CopController || other.UserData is CivvieController)
                {
                    return false;
                }
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
            if (other == _world)
            {
                //Console.WriteLine("Velocity " + _fsBody.LinearVelocity.Y);
                if (Math.Abs(_fsBody.LinearVelocity.Y) < 1 && behaviorState == BehaviorState.Flying)
                {
                    CheckPatrolPattern();
                }
                else if (_fsBody.LinearVelocity.Y < -20 && behaviorState == BehaviorState.Flying)
                {
                    this.behaviorState = BehaviorState.Dead;
                    copSprite.SetAnimationState(CopSprite.AnimationState.Dead);
                }
            }

            if (other is CivvieController)
            {
                var otherCivvie = other as CivvieController;
                if (otherCivvie._fsBody.LinearVelocity.Length() > 50)
                {
                    otherCivvie.Die();
                    this.Die();
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
            this.patrolAccumulator += time.ElapsedGameTime;

            if (Math.Abs(_fsBody.LinearVelocity.Y) > 1 &&  behaviorState != BehaviorState.Dead) { 
                behaviorState = BehaviorState.Flying;
                copSprite.SetAnimationState(CopSprite.AnimationState.Flying);
            }

            //patrols
            if (behaviorState == BehaviorState.PatrollingLeft)
            {
                copSprite.SetAnimationState(CopSprite.AnimationState.WalkingLeft);
                _fsBody.LinearVelocity = new Vector2(-20, _fsBody.LinearVelocity.Y);
                CheckPatrolPattern();
            }
            if (behaviorState == BehaviorState.PatrollingRight)
            {
                copSprite.SetAnimationState(CopSprite.AnimationState.WalkingRight);
                _fsBody.LinearVelocity = new Vector2(20, _fsBody.LinearVelocity.Y);
                CheckPatrolPattern();
            }
        }

        private void CheckPatrolPattern()
        {
            if (patrolAccumulator > this.patrolDuration)
            {
                patrolDuration = TimeSpan.Zero;

                if (behaviorState == BehaviorState.PatrollingLeft)
                {
                    behaviorState = BehaviorState.PatrollingRight;
                }
                else
                {
                    behaviorState = BehaviorState.PatrollingLeft;
                }

                this.patrolDuration = TimeSpan.FromSeconds(VERGEGame.rand.Next(3, 7));
            }
        }

        private void Die()
        {
            this.behaviorState = BehaviorState.Dead;
            copSprite.SetAnimationState(CopSprite.AnimationState.Dead);
        }

        private TimeSpan patrolAccumulator { get; set; }

        private TimeSpan patrolDuration { get; set; }
    }
}
