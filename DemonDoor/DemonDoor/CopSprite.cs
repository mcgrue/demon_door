using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using XNAVERGE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DemonDoor
{
    class CopSprite
    {
        public Texture2D texture { get; set; }
        private Filmstrip currentAnimation;

        public Sprite Sprite { get; private set; }

        public enum AnimationState
        {
            Idle, WalkingRight, WalkingLeft, Flying, Dead, Aiming, Shooting
        }

        public CopSprite(SpriteBasis sb)
        {
            animationAtlas = new Dictionary<AnimationState, Filmstrip>();
            animationAtlas[AnimationState.Idle] = createFilmstrip(0);
            animationAtlas[AnimationState.WalkingRight] = createFilmstrip(new[] { 1, 0, 2, 0 }, true);
            animationAtlas[AnimationState.WalkingLeft] = createFilmstrip(new[] { 3, 7, 4, 7 }, true);
            animationAtlas[AnimationState.Flying] = createFilmstrip(5);
            animationAtlas[AnimationState.Dead] = createFilmstrip(6);
            animationAtlas[AnimationState.Aiming] = createFilmstrip(8);
            animationAtlas[AnimationState.Shooting] = createFilmstrip(9);

            this.currentAnimation = animationAtlas[AnimationState.Idle];
            this.Sprite = new Sprite(sb, currentAnimation);
        }

        public void SetAnimationState(AnimationState state)
        {
            this.currentAnimation = animationAtlas[state];
            Sprite.set_animation(currentAnimation);
        }

        /// <summary>
        /// Creates a filmstrip with a single animation frame
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private Filmstrip createFilmstrip(int frame)
        {
            return createFilmstrip(new[] { frame });
        }
        /// <summary>
        /// Creates a filmstrip with a list of animation frames
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        private Filmstrip createFilmstrip(IList<int> frames,  bool randomizedStartFrame = false)
        {
            return new Filmstrip(new Point(16, 16), frames, 150, randomizedStartFrame);
        }

        private Dictionary<AnimationState, Filmstrip> animationAtlas;
    }
}
