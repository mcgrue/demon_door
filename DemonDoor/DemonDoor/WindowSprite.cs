using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using XNAVERGE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DemonDoor {

    

    class WindowSprite {

        public enum AnimationState {
            NormalIdle, Breaking, AngryDude, DyingDude, EmptyIdle
        }

        private Dictionary<AnimationState, Filmstrip> animationAtlas;

        public Texture2D texture { get; set; }
        private Filmstrip currentAnimation;
        public AnimationState CurrentState { get; private set; }

        public Sprite Sprite { get; private set; }

        public WindowSprite( SpriteBasis sb ) {
            animationAtlas = new Dictionary<AnimationState, Filmstrip>();
            animationAtlas[AnimationState.NormalIdle] = createFilmstrip( 0 );
            animationAtlas[AnimationState.Breaking] = createFilmstrip( new[] { 1, 2, 3 } );
            animationAtlas[AnimationState.Breaking].FrameDuration = 60;
            animationAtlas[AnimationState.AngryDude] = createFilmstrip( new[] { 4,5 } );
            animationAtlas[AnimationState.AngryDude].FrameDuration = 150;
            animationAtlas[AnimationState.DyingDude] = createFilmstrip( new[] { 6, 7 } );
            animationAtlas[AnimationState.DyingDude].FrameDuration = 150;
            animationAtlas[AnimationState.EmptyIdle] = createFilmstrip( 8 ); 

            WindowSprite that = this;

            animationAtlas[AnimationState.Breaking].OnEnd = ( Filmstrip fs ) => {

                that.SetAnimationState( AnimationState.AngryDude );

                return this.currentAnimation.FinishProcessAnimation( 0 );
            };

            animationAtlas[AnimationState.DyingDude].OnEnd = ( Filmstrip fs ) => {
                that.SetAnimationState( AnimationState.EmptyIdle );

                return this.currentAnimation.FinishProcessAnimation( 0 );
            };

            this.currentAnimation = animationAtlas[AnimationState.NormalIdle];
            this.Sprite = new Sprite(sb, currentAnimation);
        }

        private void _SetAnimationStateHelper(AnimationState state) {
            this.CurrentState = state;
            this.currentAnimation = animationAtlas[state];
            Sprite.set_animation( currentAnimation );
            Sprite.cur_filmstrip.ResetAnimation();
        }

        public void SetAnimationState( AnimationState state ) {
            _SetAnimationStateHelper(state);    
        }

        public void SetAnimationState( AnimationState state, int frame ) {
            _SetAnimationStateHelper(state);
            Sprite.cur_step = frame;
        }

        private Filmstrip createFilmstrip(int frame) {
            return createFilmstrip(new[] { frame });
        }

        private Filmstrip createFilmstrip(IList<int> frames,  bool randomizedStartFrame = false ) {
            return new Filmstrip(new Point(9, 19), frames, 150, randomizedStartFrame);
        }     
    }
}
