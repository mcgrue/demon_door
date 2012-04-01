using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAVERGE {

    /// What happens when you don't think hard about the bigger picture and you need something when tired?  This.
    public delegate Rectangle FilmstripEndDelegate( Filmstrip f );

    public class Filmstrip {

        Point frameSize;
        IList<int> frames;
        DateTime startTime;
        int frameDuration;
        int frameOffset;

        public FilmstripEndDelegate OnEnd = null;

        /// <summary>
        /// Defines a filmstrip animation.
        /// Uses an animation strip laid out in a single row.
        /// </summary>
        /// <param name="frameDimensions">Width and height of the aniation frames</param>
        /// <param name="frames">The ordered list of animation frames to show (0-based)</param>
        /// <param name="frameDurationMillis">Amount of time (in milliseconds) to show each frame</param>
        public Filmstrip(Point frameDimensions, IList<int> fr, int frameDurationMillis, bool randomizeStartFrame = false) {

            this.frameSize = frameDimensions;
            this.frames = fr;
            this.frameDuration = frameDurationMillis;
            this.startTime = DateTime.Now;
            if (randomizeStartFrame) { RandomizeCurrentFrame(); }
        }

        public void RandomizeCurrentFrame() {
            startTime -= TimeSpan.FromMilliseconds(VERGEGame.rand.Next(0, 1000));
        }

        public Rectangle ProcessAnimation() {

            TimeSpan timeSinceAnimationStarted = DateTime.Now - startTime;
            int animationIndex = (int)(timeSinceAnimationStarted.TotalMilliseconds / frameDuration);

            if( OnEnd != null && timeSinceAnimationStarted.TotalMilliseconds > frameDuration * frames.Count ) {
                return OnEnd( this  );
            } else {

                animationIndex %= frames.Count; //loop animation

                Rectangle result = new Rectangle(0, 0, frameSize.X, frameSize.Y);
                result.X = frameSize.X * frames[animationIndex];

                return result;
            }
        }
    }
}
