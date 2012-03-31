using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FsWorld = FarseerPhysics.Dynamics.World;

namespace DemonDoor
{
    class Gun
    {
        public Gun(World w, Vector2 r, Vector2 size)
        {
            

            //BodyDef def = new BodyDef();
            //def.Position.Set(r.X, r.Y);

            //_boxBody = w.AddBody(def);

            //PolygonDef shape = new PolygonDef();
            //shape.SetAsBox(size.X, size.Y);
            //shape.IsSensor = true;

            //_boxBody.CreateShape(shape);
            //_boxBody.SetMass(new MassData { Mass = 0.0f });
        }

        public Vector2 Impulse { get; set; }

        private Body _boxBody;
    }
}
