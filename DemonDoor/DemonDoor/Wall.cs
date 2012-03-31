using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace DemonDoor
{
    class Wall
    {
        public Wall(World w, float x, float sgn)
        {
            Vector2 center = new Vector2 { X = x - (10.0f * sgn), Y = 500.0f };

            _fsBody = w.NewBody();
            _fsBody.BodyType = BodyType.Static;
            _fsBody.Position = center;

            PolygonShape wallShape = new PolygonShape(0);
            wallShape.SetAsBox(10f, 1000f);

            Fixture f = _fsBody.CreateFixture(wallShape);
        }

        private Body _fsBody;
    }
}
