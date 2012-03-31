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
        Rectangle frameSize;
        IList<int> frames;
        TimeSpan frameDuration;
        DateTime startTime;
        
        public Filmstrip(Rectangle frameSize, IList<int> frames, TimeSpan frameDuration)
        {
            this.frameSize = frameSize;
            this.frames = frames;
            this.frameDuration = frameDuration;
            this.startTime = DateTime.Now;
        }

        public Rectangle ProcessAnimation()
        {
            TimeSpan timeSinceAnimationStarted = DateTime.Now - startTime;
            int animationIndex = (int)(timeSinceAnimationStarted.TotalMilliseconds / frameDuration.TotalMilliseconds);
            animationIndex %= frames.Count; //loop animation

            Rectangle result = frameSize;
            result.X = frameSize.Width * animationIndex;

            return result;
        }
    }
}
