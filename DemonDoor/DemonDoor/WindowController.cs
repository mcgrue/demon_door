using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAVERGE;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace DemonDoor
{
    class WindowController : IDrawableThing, ICollidable {

        private WindowSprite sprite;

        public WindowController( World w, Vector2 r, Vector2 size, WindowSprite s ) {
            _fsBody = w.NewBody();
            _fsBody.Position = r;

            PolygonShape shape = new PolygonShape(1.0f);
            shape.SetAsBox(size.X * 2, size.Y * 2);

            _fsFixture = _fsBody.CreateFixture(shape, this);
            _fsFixture.IsSensor = true;

            _fsFixture.OnCollision += BehaviorCollided;

            sprite = s;
        }

        private bool BehaviorCollided(Fixture f1, Fixture f2, Contact contact) {
            Fixture self = null, other = null;

            if (f1 == _fsFixture) {
                self = f1;
                other = f2;
            } else if (f2 == _fsFixture) {
                self = f2;
                other = f1;
            }

            if (other.UserData is ICollidable) {
                this.Collided(other.UserData as ICollidable);
                (other.UserData as ICollidable).Collided(this);
            }

            return false;
        }

        public void Collided(ICollidable other) {
            if( other is CivvieController ) {
                var civvie = other as CivvieController;

                if (civvie._fsBody.LinearVelocity.Length() > 100f) { 
                    if( sprite.CurrentState == WindowSprite.AnimationState.NormalIdle ) {
                        sprite.SetAnimationState( WindowSprite.AnimationState.Breaking, 0 );
                        // TODO "windowbreak sound"
                        // TODO "award points" break
                    } else if( sprite.CurrentState == WindowSprite.AnimationState.AngryDude ) {
                        sprite.SetAnimationState( WindowSprite.AnimationState.DyingDude, 0 );
                        /// TODO "windowdie sound"
                        /// TODO "award points" die
                    }
                }
            }
        }

        public Vector2 Position {
            get {
                return _fsBody.Position;
            }
        }

        
        public int GetX() { return sprite.Sprite.x; }
        public int GetY() { return sprite.Sprite.y; }

        public void UpdateWindowState( GameTime gameTime ) {

            /*
            if( Game1.game.action.cancel.pressed && sprite.CurrentState == DemonSprite.AnimationState.Idle ) {
                sprite.SetAnimationState( DemonSprite.AnimationState.Disappearing, 0 );
            }

            if( !Game1.game.action.cancel.down && sprite.CurrentState == DemonSprite.AnimationState.Hidden ) {
                sprite.SetAnimationState( DemonSprite.AnimationState.Reappearing, 0 );
            }

            if( Game1.game.action.confirm.pressed && sprite.CurrentState == DemonSprite.AnimationState.Idle ) {
                sprite.SetAnimationState( DemonSprite.AnimationState.Pressing );
            }

            if( Game1.game.action.confirm.released && sprite.CurrentState == DemonSprite.AnimationState.Pressing ) {
                sprite.SetAnimationState( DemonSprite.AnimationState.Idle );
            }
            */
        }

        RenderDelegate _myDrawDelegate;
        public RenderDelegate GetDrawDelegate() {
            if( _myDrawDelegate != null ) {
                return _myDrawDelegate;
            }

            _myDrawDelegate = ( int x, int y ) => {

                Vector2 screen = Coords.Physics2Screen(
                    new Vector2 { X = Position.X, Y = Position.Y }
                );

                sprite.Sprite.x = (int)screen.X - 10;
                sprite.Sprite.y = (int)screen.Y - 9;
                sprite.Sprite.Draw();
            };

            return _myDrawDelegate;
        }

        public Vector2 Impulse { get; set; }

        private Fixture _fsFixture;
        private Body _fsBody;

        private ISet<Fixture> _alreadyShot = new HashSet<Fixture>();
    }
}
