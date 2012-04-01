using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using XNAVERGE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DemonDoor {

    class DoorSprite {

        public Texture2D texture { get; set; }
        private Filmstrip currentAnimation;

        public Sprite Sprite { get; private set; }

        public enum AnimationState {
            Stopped, Slow, Fast
        }

        public DoorSprite( SpriteBasis sb ) {
            animationAtlas = new Dictionary<AnimationState, Filmstrip>();
            animationAtlas[AnimationState.Stopped] = createFilmstrip( new[] { 0 } );
            animationAtlas[AnimationState.Slow] = createFilmstrip(new[] { 0, 1, 2 });
            animationAtlas[AnimationState.Fast] = createFilmstrip(new[] { 3, 4 });

            DrawDoor = (int x, int y) => {
                Sprite.Update();
                Sprite.Draw();
            };

            this.currentAnimation = animationAtlas[AnimationState.Slow];
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
            return new Filmstrip(new Point(38, 24), frames, 150);
        }

        public RenderDelegate DrawDoor { get; set; }

        private Dictionary<AnimationState, Filmstrip> animationAtlas;
    }
}
