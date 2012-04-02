using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAVERGE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DemonDoor
{
    class Level1Screen : Screen
    {
        public McGrenderStack mcg;

        public WindowController[] windowControllerList;

        private World _world;

        private DoorController _gun;
        private DemonController _evilDemon;

        internal override string BgBgBg
        {
            get { return "demon_door"; }
        }

        internal override void Load()
        {
            Game1 game1 = (Game1)Game1.game;
            //Vector2[] verts = new [] {
            //    new Vector2 { X = -100, Y = 0 },
            //    new Vector2 { X = -70, Y = 0 },
            //    new Vector2 { X = -100, Y = 30 }
            //};

            //Wall _wallTri = new Wall(_world, verts);
            McgNode rendernode;
            Vector2 floor = Coords.Screen2Physics(new Vector2 { X = 0, Y = 215 });
            _world = new World(new Vector2 { X = 0, Y = -10 }, floor.Y);

            Vector2 _666pos = Coords.Screen2Physics(new Vector2 { X = 280, Y = 218 });
            Vector2 _666size = Coords.Screen2Physics(new Vector2 { X = 100, Y = 210 }, true);

            Wall _wall0 = new Wall(_world, -100, 1);

            Vector2[] verts = new[] {
                Coords.Screen2Physics(new Vector2 { X = 340, Y = 32 }),
                Coords.Screen2Physics(new Vector2 { X = 280, Y = 32 }),
                Coords.Screen2Physics(new Vector2 { X = 280, Y = 200 }),
                Coords.Screen2Physics(new Vector2 { X = 340, Y = 200 }),
            };

            Wall _wall1 = new Wall(_world, verts);
            //Wall _wall1 = new Wall(_world, _666pos.X, -1);

            List<SpriteBasis> civilianList = new List<SpriteBasis>();
            
            for( int i = 0; i < game1.im_civvies.Length; i++ ) {
                SpriteBasis civSpriteBasis = new SpriteBasis( 16, 16, 7, 7 );
                civSpriteBasis.image = game1.im_civvies[i];
                civilianList.Add( civSpriteBasis );
            }

            SpriteBasis copSpriteBasis = new SpriteBasis(16, 16, 10, 10);
            copSpriteBasis.image = game1.im_cop;

            mcg = new McGrenderStack();
            Game1.game.setMcGrender(mcg);

            mcg.AddLayer( "skybox" );
            mcg.AddLayer( "clouds" );
            mcg.AddLayer( "background" );
            mcg.AddLayer("corpses");

            McgLayer l = mcg.GetLayer( "skybox" );
            /// this is wrong.
            Rectangle rectTitle = new Rectangle( 0, 0, 320, 240 );
            rendernode = l.AddNode(
                new McgNode( game1.im_skybox, rectTitle, l, 0, 0 )
            );

            l = mcg.GetLayer("background");
            rendernode = l.AddNode(
                new McgNode(game1.im_stage, rectTitle, l, 0, 0)
            );

            l = mcg.GetLayer( "clouds" );

            for( int i = 0; i < 40; i++ ) {
                int x = VERGEGame.rand.Next( -400, 350 );
                int y = VERGEGame.rand.Next(0,150);
                int d = VERGEGame.rand.Next(50000,200000);
                rendernode = l.AddNode(
                    new McgNode( game1.im_clouds[i%9], null, l, x,y ,600,y,d )
                );
            }
            /// this all should be encapsulated eventually.  CORPSEMAKER.
            l = mcg.GetLayer("corpses");

            var doorSpriteBasis = new SpriteBasis(38, 24, 5, 5);
            doorSpriteBasis.image = game1.im_door;
            var doorSprite = new DoorSprite(doorSpriteBasis);
            _gun = new DoorController(
                _world,
                Coords.Screen2Physics(new Vector2 { X = 32, Y = 206 }),
                Coords.Screen2Physics(new Vector2 { X = 19, Y = 12 }, true),
                doorSprite
            );
            _gun.Impulse = new Vector2 { X = -10, Y = 10 };

            rendernode = l.AddNode(
                new McgNode(_gun, l, 60, 200)
            );

            var demonSpriteBasis = new SpriteBasis(9, 19, 7, 7);
            demonSpriteBasis.image = game1.im_demon;
            var demonSprite = new DemonSprite( demonSpriteBasis );
            _evilDemon = new DemonController( _world,
                Coords.Screen2Physics(new Vector2 { X = 286, Y = 26 }),
                Coords.Screen2Physics(new Vector2 { X = 5, Y = 10 }, true),
                demonSprite
            );
            rendernode = l.AddNode(
                new McgNode(_evilDemon, l, -1, -1)
            );

            List<WindowController> windowList = new List<WindowController>();
            var windowSpriteBasis = new SpriteBasis( 14, 14, 9, 9 );
            windowSpriteBasis.image = game1.im_windowbreak;
            for( int i = 0; i < 6; i++ ) {
                var windowSprite = new WindowSprite( windowSpriteBasis );
                var windowController = new WindowController( _world,
                    Coords.Screen2Physics( new Vector2 { X = 297, Y = ( 53 + ( i * 22 ) + ( i > 1 ? 1 : 0 ) + ( i > 2 ? 1 : 0 ) ) } ),
                    Coords.Screen2Physics( new Vector2 { X = 7, Y = 7 }, true ),
                    windowSprite
                );
                rendernode = l.AddNode(
                    new McgNode( windowController, l, -1, -1 )
                );

                windowList.Add( windowController );
            }

            windowControllerList = windowList.ToArray();

            //spawn guys
            Vector2 spawnerR = Coords.Screen2Physics(new Vector2 { X = 325, Y = 218 });
            var civvieSpawner = new CivvieSpawner( _world, l, spawnerR, TimeSpan.FromSeconds( 1 ), civilianList.ToArray(), 1000 );
            l.AddNode(new McgNode(civvieSpawner, l, 80, 20));

            //spawn cops
            var copSpawner = new CopSpawner(_world, l, spawnerR, TimeSpan.FromSeconds(1), copSpriteBasis, 1000);
            l.AddNode(new McgNode(copSpawner, l, 80, 20));
        }

        Vector2 _aimPoint = Vector2.UnitX;
        string _lastDoorCue = null;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        internal override void Update(GameTime gameTime)
        {
            Vector2 leftAnalog = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;

            leftAnalog = Vector2.Clamp(leftAnalog, Vector2.Zero, new Vector2 { X = 1, Y = 1 });

            if (leftAnalog.Length() < 1e-10)
            {
                _aimPoint = new Vector2 { X = 1, Y = 1 };
            }
            else
            {
                _aimPoint = leftAnalog;
            }

            //Console.Out.WriteLine(_aimPoint);

            float oldImpulse = _gun.GunImpulse;

            {
                // update gun impulse.
                _gun.UpdateGunImpulse(gameTime);
                _evilDemon.UpdateDemonState( gameTime );

                Vector2 dir = _aimPoint;
                dir.Normalize();

                _gun.Impulse = dir * _gun.GunImpulse;
            }

            string nextDoorCue = null;

            switch (_gun.Speed)
            {
                case DoorController.DoorSpeed.Fast:
                    nextDoorCue = "door_fast";
                    break;
                case DoorController.DoorSpeed.Medium:
                    nextDoorCue = "door_med";
                    break;
                case DoorController.DoorSpeed.Slow:
                    nextDoorCue = "door_slow";
                    break;
                case DoorController.DoorSpeed.Stopping:
                    nextDoorCue = "door_stop";
                    break;
            }

            if (nextDoorCue != _lastDoorCue && nextDoorCue != null)
            {
                Game1.game.PlayCue(nextDoorCue);
            }

            _lastDoorCue = nextDoorCue;


            _world.Simulate(gameTime);
            mcg.Update(gameTime);

            //Console.Out.WriteLine("@{3}: ({0}, {1}), {2}", _test.Position.X, _test.Position.Y, _test.Theta, gameTime.TotalGameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        internal override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            Game1 game1 = (Game1)Game1.game;
            string doorSpeedDesc = string.Format("door speed: {0}", _gun.DoorSpeedDescription);
            Vector2 size = game1.ft_hud24.MeasureString(doorSpeedDesc);

            batch.DrawString(game1.ft_hud24, doorSpeedDesc, new Vector2 { X = (640 - size.X) / 2, Y = 480 - 5 - size.Y }, Color.White);
        }
    }
}
