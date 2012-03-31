using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAVERGE
{
    public class Filmstrip
    {
        Point frameSize;
        IList<int> frames;
        int frameDuration;
        DateTime startTime;
        
        public Filmstrip(Point frameDimensions, IList<int> frames, int frameDurationMillis)
        {
            this.frameSize = frameDimensions;
            this.frames = frames;
            this.frameDuration = frameDurationMillis;
            this.startTime = DateTime.Now;
        }

        public Rectangle ProcessAnimation()
        {
            TimeSpan timeSinceAnimationStarted = DateTime.Now - startTime;
            int animationIndex = (int)(timeSinceAnimationStarted.TotalMilliseconds / frameDuration);
            animationIndex %= frames.Count; //loop animation

            Rectangle result = new Rectangle(0, 0, frameSize.X, frameSize.Y);
            result.X = frameSize.X * animationIndex;

            return result;
        }
    }
}
