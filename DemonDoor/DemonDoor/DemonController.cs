using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAVERGE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Common;

namespace DemonDoor
{
    

    class DemonController : IDrawableThing, ICollidable {

        public static DemonController TheDemon;

        public DemonController(World w, Vector2 r, Vector2 size, DemonSprite s) {
            _fsBody = w.NewBody();
            _fsBody.Position = r;

            PolygonShape shape = new PolygonShape(1.0f);
            shape.SetAsBox(size.X, size.Y);

            _fsFixture = _fsBody.CreateFixture(shape, this);
            _fsFixture.IsSensor = true;

            _fsFixture.OnCollision += BehaviorCollided;

            sprite = s;

            TheDemon = this;
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

                if( ( sprite.CurrentState == DemonSprite.AnimationState.Idle || sprite.CurrentState == DemonSprite.AnimationState.Pressing ) ) {
                    ( Game1.game as Game1 ).LoadLevel( "gameover" );
                } else {
                    /// do something if a "miss" happens?  disintegrate the corpse?
                }
            }
        }

        public Vector2 Position {
            get {
                return _fsBody.Position;
            }
        }

        private DemonSprite sprite;

        public int GetX() { return sprite.Sprite.x; }
        public int GetY() { return sprite.Sprite.y; }

        public void UpdateDemonState( GameTime gameTime ) {

            //Console.WriteLine( "DEMON State: " + sprite.CurrentState.ToString() );

            if(GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed && sprite.CurrentState == DemonSprite.AnimationState.Idle ) {
                sprite.SetAnimationState( DemonSprite.AnimationState.Disappearing, 0 );
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Released && sprite.CurrentState == DemonSprite.AnimationState.Hidden)
            {
                sprite.SetAnimationState( DemonSprite.AnimationState.Reappearing, 0 );
            }

            if( Game1.game.action.confirm.pressed && sprite.CurrentState == DemonSprite.AnimationState.Idle ) {
                sprite.SetAnimationState( DemonSprite.AnimationState.Pressing );
            }

            if( Game1.game.action.confirm.released && sprite.CurrentState == DemonSprite.AnimationState.Pressing ) {
                sprite.SetAnimationState( DemonSprite.AnimationState.Idle );
            }
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

                if (Utility.DrawHitboxes)
                {
                    Vertices v = (_fsFixture.Shape as PolygonShape).Vertices;

                    _.setDrawTarget(Game1.game.spritebatch);

                    int x0 = int.MaxValue, x1 = int.MinValue, y0 = int.MaxValue, y1 = int.MinValue;

                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 r = Coords.Physics2Screen(v[i] + Position);

                        if (r.X < x0) x0 = (int)r.X;
                        if (r.X > x1) x1 = (int)r.X;
                        if (r.Y < y0) y0 = (int)r.Y;
                        if (r.Y > y1) y1 = (int)r.Y;
                    }

                    _.DrawRect(x0, y0, x1, y1, Color.Green);
                }
            };

            return _myDrawDelegate;
        }

        public Vector2 Impulse { get; set; }

        private Fixture _fsFixture;
        private Body _fsBody;

        private ISet<Fixture> _alreadyShot = new HashSet<Fixture>();
    }
}
