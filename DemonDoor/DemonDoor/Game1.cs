using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using XNAVERGE;

namespace DemonDoor {

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : VERGEGame {

        public McGrenderStack mcg;
        Texture2D civvie, title;

        private World _world;
        private Corpse _test;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            _world = new World(new Vector2 { X = 0, Y = -10 });
            _test = new Corpse(_world, new Vector2 { X = 0, Y = 100 });
            McgNode rendernode;

            // TODO: Add your initialization logic here
            civvie = Content.Load<Texture2D>( "civilian_01" );
            title = Content.Load<Texture2D>( "title" );

            SpriteBasis sb = new SpriteBasis( 16, 16, 7, 7 );
            
            sb.image = civvie;
            Sprite sprite = new Sprite(sb, new Filmstrip(new Point(16, 16), new[] { 1, 2, 3, 4, 5 }, TimeSpan.FromMilliseconds(100)));

            mcg = new McGrenderStack();
            mcg.AddLayer( "menu" );
            mcg.AddLayer( "textbox" );
            this.setMcGrender( mcg );

            McgLayer l = mcg.GetLayer( "menu" );

            /// this is wrong.
            Rectangle rectTitle = new Rectangle( 0, 0, 640, 480 );
            rendernode = l.AddNode(
                new McgNode( title, rectTitle, l, 0, 0 )
            );

            Rectangle rect = new Rectangle( 0, 0, 122, 16 );

            rendernode = l.AddNode(
                new McgNode( civvie, rect, l, 0, 0 )
            );

            RenderDelegate drawCivvie = ( int x, int y ) => {
                sprite.Update();
                sprite.Draw();
            };

            rendernode = l.AddNode(
                new McgNode( drawCivvie, l, 100, 100 )
            );

            /*
            rendernode = l.AddNode(
                            new McgNode(sp
                        );
            */

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spritebatch = new SpriteBatch( GraphicsDevice );

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        int systime;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update( GameTime gameTime ) {
            // Allows the game to exit
            if( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed )
                this.Exit();

            _world.Simulate(gameTime);

            Console.Out.WriteLine("({0}, {1}), {2}", _test.Position.X, _test.Position.Y, _test.Theta);
            systime = gameTime.TotalGameTime.Milliseconds;

            // TODO: Add your update logic here

            base.Update( gameTime );
        } 

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime ) {
            GraphicsDevice.Clear( Microsoft.Xna.Framework.Color.LimeGreen );


            Rectangle rect = new Rectangle( 10, 30, 112, 16 );

            spritebatch.Begin();
            spritebatch.Draw( civvie, rect, Microsoft.Xna.Framework.Color.White );
            spritebatch.End();

            base.Draw( gameTime );
        }
    }
}
