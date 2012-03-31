using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAVERGE {

    public class Filmstrip {
        Point frameSize;
        IList<int> frames;
        int frameDuration;
        DateTime startTime;
        
        /// <summary>
        /// Defines a filmstrip animation.
        /// Uses an animation strip laid out in a single row.
        /// </summary>
        /// <param name="frameDimensions">Width and height of the aniation frames</param>
        /// <param name="frames">The ordered list of animation frames to show (0-based)</param>
        /// <param name="frameDurationMillis">Amount of time (in milliseconds) to show each frame</param>
        public Filmstrip(Point frameDimensions, IList<int> frames, int frameDurationMillis) {

            this.frameSize = frameDimensions;
            this.frames = frames;
            this.frameDuration = frameDurationMillis;
            this.startTime = DateTime.Now;
        }

        public Rectangle ProcessAnimation() {

            TimeSpan timeSinceAnimationStarted = DateTime.Now - startTime;
            int animationIndex = (int)(timeSinceAnimationStarted.TotalMilliseconds / frameDuration);
            
            animationIndex %= frames.Count; //loop animation

            Rectangle result = new Rectangle(0, 0, frameSize.X, frameSize.Y);
            result.X = frameSize.X * frames[animationIndex];

            return result;
        }
    }
}
