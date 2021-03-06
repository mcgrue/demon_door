﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAVERGE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DemonDoor
{
    class TitleScreen : Screen
    {
        public McGrenderStack mcg;
        private bool _allowExit;

        internal override string BgBgBg
        {
            get
            {
                return "demon_door_intro";
            }
        }

        internal override void Load()
        {
            _allowExit = false;
            Game1 game1 = (Game1)Game1.game;
            //Vector2[] verts = new [] {
            //    new Vector2 { X = -100, Y = 0 },
            //    new Vector2 { X = -70, Y = 0 },
            //    new Vector2 { X = -100, Y = 30 }
            //};

            //Wall _wallTri = new Wall(_world, verts);
            McgNode rendernode;

            mcg = new McGrenderStack();
            Game1.game.setMcGrender(mcg);

            mcg.AddLayer("background");

            McgLayer l = mcg.GetLayer("background");
            /// this is wrong.
            Rectangle rectTitle = new Rectangle(0, 0, 320, 240);
            rendernode = l.AddNode(
                new McgNode(game1.im_title, rectTitle, l, 0, 0)
            );

        }

        internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_allowExit && Game1.game.action.confirm.pressed)
            {
                Game1 game1 = Game1.game as Game1;

                game1.LoadLevel("level1");
            }

            if (Game1.game.action.confirm.released)
            {
                _allowExit = true;
            }
        }

        internal override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1 game1 = (Game1)Game1.game;
            string startInstructions = null;
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                startInstructions = string.Format("press A to start");
            }
            else
            {
                startInstructions = string.Format("mash space bar to start");
            }
            Vector2 size = game1.ft_hud24.MeasureString(startInstructions);

            batch.DrawString(game1.ft_hud24, startInstructions, new Vector2 { X = (640 - size.X) / 2, Y = 400 }, Color.White);
        }

    }
}
