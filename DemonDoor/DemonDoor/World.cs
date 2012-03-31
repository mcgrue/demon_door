using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using BoxWorld = Box2DX.Dynamics.World;

namespace DemonDoor
{
    class World
    {
        public World(Vector2 gravity)
        {
            AABB aabb = new AABB();
            aabb.LowerBound = new Vec2(-10000f, -10000f);
            aabb.UpperBound = new Vec2(10000f, 10000f);

            _boxWorld = new BoxWorld(aabb, Utility.X2BVec2(gravity), true);
        }

        private BoxWorld _boxWorld;
    }
}
