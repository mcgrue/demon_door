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
    class GameOverScreen : Screen
    {
        public McGrenderStack mcg;
        private string _param;

        public GameOverScreen(string param)
        {
            _param = param;
        }

        internal override void Load() {
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
            ///// this is wrong.
            Rectangle rectTitle = new Rectangle(0, 0, 320, 240);
            rendernode = l.AddNode(
                new McgNode(game1.im_gameover, rectTitle, l, 0, 0)
            );

        }

        internal override string BgBgBg
        {
            get
            {
                return "gameover";
            }
        }

        internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B))
            {
                Game1 game1 = Game1.game as Game1;

                game1.LoadLevel("title");
            }
        }

        private void DrawDeathMessage(SpriteBatch batch, string text, int y, Color color)
        {
            Game1 game1 = (Game1)Game1.game;
            Vector2 size = game1.ft_hud24.MeasureString(text);

            batch.DrawString(game1.ft_hud24, text, new Vector2 { X = 30, Y = y }, color);
        }

        internal override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1 game1 = (Game1)Game1.game;

            Color color = Color.White;
            if (_param == "corpse")
            {
                DrawDeathMessage(batch, "You were slain by a flying corpse,", 20, color);
                DrawDeathMessage(batch, "borne aloft on the wings of your", 60, color);
                DrawDeathMessage(batch, "revolving door.", 100, color);
            }
            else if (_param == "bullet")
            {
                DrawDeathMessage(batch, "You were killed by a cop on the beat", 20, color);
                DrawDeathMessage(batch, "trying to save his neighborhood", 60, color);
                DrawDeathMessage(batch, "from your onslaught.", 100, color);
            }

            DrawDeathMessage(batch, "Press B to try again.", 180, color);

            //DrawCentered(batch, "- game over -", 360, Color.Red);
        }

    }
}
