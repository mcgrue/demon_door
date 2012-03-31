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
    class Civvie
    {
        public Texture2D texture { get; set; }
        private Filmstrip currentAnimation;

        public Sprite Sprite { get; private set; }

        public enum AnimationState
        {
            Idle, WalkingRight, WalkingLeft, Flying, Dead
        }

        public Civvie(SpriteBasis sb)
        {
            animationAtlas = new Dictionary<AnimationState, Filmstrip>();
            animationAtlas[AnimationState.Idle] = createFilmstrip(0);
            animationAtlas[AnimationState.WalkingRight] = createFilmstrip(new[] { 1, 0, 2, 0 });
            animationAtlas[AnimationState.WalkingLeft] = createFilmstrip(new[] { 3, 4 });
            animationAtlas[AnimationState.Flying] = createFilmstrip(5);
            animationAtlas[AnimationState.Dead] = createFilmstrip(6);

            DrawCivvie = (int x, int y) =>
            {
                Sprite.Update();
                Sprite.Draw();
            };

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
        private Filmstrip createFilmstrip(IList<int> frames)
        {
            return new Filmstrip(new Point(16, 16), frames, 150);
        }

        public RenderDelegate DrawCivvie { get; set; }

        private Dictionary<AnimationState, Filmstrip> animationAtlas;
    }
}
