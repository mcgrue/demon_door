
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

using XNAVERGE;

namespace DemonDoor
{

    class Corpse : IDrawableThing, ICollidable
    {
        Sprite myCorpse = null;
        Vector2 screen;

        private Body _fsBody;
        private Fixture _fsFixture;

        public Corpse(World w, Vector2 r0, Sprite sprite)
        {
            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Dynamic;
            _fsBody.Position = r0;

            CircleShape shape = new CircleShape(2f, 1.0f);
            
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

                this.screen = Game1.Physics2Screen( new Vector2 { X = Position.X, Y = Position.Y } );

                // maybe update the screen here?

                myCorpse.x = (int)screen.X - 8;
                myCorpse.y = (int)screen.Y - 8;
                myCorpse.Draw();
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
    }
}
