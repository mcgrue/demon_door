using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FsWorld = FarseerPhysics.Dynamics.World;

using XNAVERGE;

namespace DemonDoor
{
    class Corpse : IDrawableThing
    {
        Sprite myCorpse = null;
        Vector2 screen;

        public Corpse(World w, Vector2 r0)
        {
            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Dynamic;
            _fsBody.Position = r0;

            CircleShape shape = new CircleShape(2f, 1.0f);
            
            Fixture f = _fsBody.CreateFixture(shape, this);
            f.Restitution = 0.5f;

            this.screen =  Game1.Physics2Screen( new Vector2 { X = Position.X, Y = Position.Y } );
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

                // maybe update the screen here?

                myCorpse.x = (int)screen.X - 8;
                myCorpse.y = (int)screen.Y - 8;
                myCorpse.Draw();
            };

            return _myDrawDelegate;
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
