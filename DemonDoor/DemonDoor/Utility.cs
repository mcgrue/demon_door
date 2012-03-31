using Microsoft.Xna.Framework;
using Box2DX.Common;

namespace DemonDoor
{
    static class Utility
    {
        public static Vector2 B2XVec2(Vec2 boxVec)
        {
            return new Vector2(boxVec.X, boxVec.Y);
        }

        public static Vec2 X2BVec2(Vector2 xnaVec)
        {
            return new Vec2(xnaVec.X, xnaVec.Y);
        }
    }
}