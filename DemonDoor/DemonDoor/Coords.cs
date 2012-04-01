using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace DemonDoor
{
    static class Coords
    {

        private const int Width = 320;
        private const int Height = 240;

        private const float vpX0 = -100, vpX1 = 100, vpY0 = 100, vpY1 = 0;
        private const float vpXZoom = Width / (vpX1 - vpX0);
        private const float vpYZoom = Height / (vpY1 - vpY0);

        public static Vector2 Physics2Screen(Vector2 physics, bool distance = false)
        {
            if (!distance)
            {
                return new Vector2 { X = (physics.X - vpX0) * vpXZoom, Y = (physics.Y - vpY0) * vpYZoom };
            }
            else
            {
                return new Vector2 { X = Math.Abs(physics.X * vpXZoom), Y = Math.Abs(physics.Y * vpYZoom) };
            }
        }

        public static Vector2 Screen2Physics(Vector2 screen, bool distance = false)
        {
            if (!distance)
            {
                return new Vector2 { X = (screen.X / vpXZoom) + vpX0, Y = (screen.Y / vpYZoom) + vpY0 };
            }
            else
            {
                return new Vector2 { X = Math.Abs(screen.X / vpXZoom), Y = Math.Abs(screen.Y / vpYZoom) };
            }
        }
    }
}
