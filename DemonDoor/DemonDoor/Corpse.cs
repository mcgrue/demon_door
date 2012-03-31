using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;

namespace DemonDoor
{
    class Corpse
    {
        public Corpse(World w, Vector2 r0)
        {
            BodyDef def = new BodyDef();
            def.Position.Set(r0.X, r0.Y);

            _boxBody = w.AddBody(def);

            CircleDef shape = new CircleDef();
            shape.Radius = 2f;
            shape.Restitution = 0.5f;

            _boxBody.CreateShape(shape);
            _boxBody.SetMass(new MassData { Mass = 1.0f });
            _boxBody.SetMassFromShapes();
        }

        public Vector2 Position
        {
            get
            {
                return Utility.B2XVec2(_boxBody.GetPosition());
            }
        }

        public float Theta
        {
            get
            {
                return _boxBody.GetAngle();
            }
        }

        private Body _boxBody;
    }
}
