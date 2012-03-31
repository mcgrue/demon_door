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

using Box2DX;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;

namespace XNAVERGE {

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : VERGEGame {

        public McGrenderStack mcg;
        Texture2D civvie, title;

        private World _world;
        private Body _ground, _box;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            McgNode rendernode;

            Vec2 gravity = new Vec2(0.0f, -10.0f);

            // set up the world
            AABB worldAabb = new AABB();
            worldAabb.LowerBound = new Vec2(-10000, -10000);
            worldAabb.UpperBound = new Vec2(10000, 10000);

            _world = new World(worldAabb, gravity, true);

            // set up the ground
            BodyDef groundDef = new BodyDef();
            groundDef.Position.Set(0f, -10f);

            _ground = _world.CreateBody(groundDef);

            PolygonDef boxDef = new PolygonDef();
            boxDef.SetAsBox(50f, 10f);

            _ground.CreateShape(boxDef);
            _ground.SetMass(new MassData { Mass = 0.0f });

            // set up the ball
            BodyDef ballDef = new BodyDef();
            ballDef.Position.Set(0f, 100f);

            _box = _world.CreateBody(ballDef);

            CircleDef circleDef = new CircleDef();
            circleDef.Radius = 2f;

            _box.CreateShape(circleDef);
            _box.SetMass(new MassData { Mass = 1.0f });
            
            civvie = Content.Load<Texture2D>( "civilian_01" );
            title = Content.Load<Texture2D>( "title" );

            SpriteBasis sb = new SpriteBasis( 16, 16, 7, 7 );
            
            sb.image = civvie;
            Sprite sprite = new Sprite(sb, new Filmstrip(new Rectangle(0, 0, 16, 16), new[] { 1, 2, 3, 4, 5 }, TimeSpan.FromMilliseconds(100)));
            

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

            _world.Step(1f / 60f, 6, 2);
            Vec2 ballPos = _box.GetPosition();
            float ballAngle = _box.GetAngle();

            Console.Out.WriteLine("({0}, {1}), {2}", ballPos.X, ballPos.Y, ballAngle);
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
