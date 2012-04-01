using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using XNAVERGE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DemonDoor {

    class DemonSprite {
        public Texture2D texture { get; set; }
        private Filmstrip currentAnimation;

        public Sprite Sprite { get; private set; }

        public enum AnimationState {
            Idle, Pressing, Disappearing, Hidden, Reappearing
        }

        public DemonSprite(SpriteBasis sb) {
            animationAtlas = new Dictionary<AnimationState, Filmstrip>();
            animationAtlas[AnimationState.Idle] = createFilmstrip( 0 );
            animationAtlas[AnimationState.Pressing] = createFilmstrip( 1 );
            animationAtlas[AnimationState.Disappearing] = createFilmstrip( new[] { 1, 2, 3, 4, 5, 6 } );
            animationAtlas[AnimationState.Hidden] = createFilmstrip( 6 );
            animationAtlas[AnimationState.Reappearing] = createFilmstrip( new[] { 6, 5, 4, 3, 2, 1} );

            this.currentAnimation = animationAtlas[AnimationState.Idle];
            this.Sprite = new Sprite(sb, currentAnimation);
        }

        public void SetAnimationState(AnimationState state) {
            this.currentAnimation = animationAtlas[state];
            Sprite.set_animation(currentAnimation);
        }

        private Filmstrip createFilmstrip(int frame) {
            return createFilmstrip(new[] { frame });
        }

        private Filmstrip createFilmstrip(IList<int> frames,  bool randomizedStartFrame = false ) {
            return new Filmstrip(new Point(9, 19), frames, 150, randomizedStartFrame);
        }

        private Dictionary<AnimationState, Filmstrip> animationAtlas;
    }
}
