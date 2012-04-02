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
    class BlurbScreen : Screen
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
            //_allowExit = false;
            Game1 game1 = (Game1)Game1.game;
            //Vector2[] verts = new [] {
            //    new Vector2 { X = -100, Y = 0 },
            //    new Vector2 { X = -70, Y = 0 },
            //    new Vector2 { X = -100, Y = 30 }
            //};

            //Wall _wallTri = new Wall(_world, verts);
            //McgNode rendernode;

            mcg = new McGrenderStack();
            Game1.game.setMcGrender(mcg);

            //mcg.AddLayer("background");

            //McgLayer l = mcg.GetLayer("background");
            ///// this is wrong.
            //Rectangle rectTitle = new Rectangle(0, 0, 320, 240);
            //rendernode = l.AddNode(
            //    new McgNode(game1.im_title, rectTitle, l, 0, 0)
            //);

        }

        TimeSpan? entryTime = null;

        internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entryTime == null)
            {
                entryTime = gameTime.TotalGameTime;
            }

            //if (_allowExit && Game1.game.action.confirm.pressed)
            //{
            //    Game1 game1 = Game1.game as Game1;

            //    game1.LoadLevel("level1");
            //}

            //if (Game1.game.action.confirm.released)
            //{
            //    _allowExit = true;
            //}
        }

        private void DrawFadingMessage(SpriteBatch batch, string text, int x, int y, int msStart, int msNow, int msDone, Color color)
        {
            Game1 game1 = (Game1)Game1.game;
            Vector2 size = game1.ft_hud24.MeasureString(text);
            Color effectiveColor = Color.Black;

            if (msNow > msDone)
            {
                effectiveColor = color;
            }
            else if (msNow > msStart)
            {
                float fade = (float)(msNow - msStart) / (float)(msDone - msStart);

                effectiveColor = new Color(fade * color.R, fade * color.G, fade * color.B);
                Console.Out.WriteLine("{0}, {1}, {2}", effectiveColor.R, effectiveColor.G, effectiveColor.B);
            }
            else
            {
                effectiveColor = Color.Black;
            }

            batch.DrawString(game1.ft_hud24, text, new Vector2 { X = x, Y = y }, effectiveColor);
        }

        internal override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1 game1 = (Game1)Game1.game;

            int msNow = (int)(gameTime.TotalGameTime - entryTime.Value).TotalMilliseconds;
            //Console.Out.WriteLine(msNow);

            Color color = Color.White;
            DrawFadingMessage(batch, "\"doors and windows, however,\nthey interest me.", 30, 40, 500, msNow, 1500, Color.White);

            DrawFadingMessage(batch, "I mean, what if, just imagine\nfor a moment", 30, 160, 1500, msNow, 2500, Color.White);
            DrawFadingMessage(batch, "that you could actually", 30, 240, 2500, msNow, 3500, Color.White);
            DrawFadingMessage(batch, "take CONTROL of a door.\"", 30, 280, 3500, msNow, 4500, Color.Red);

            DrawFadingMessage(batch, "- P.M.", 530, 360, 4500, msNow, 5500, Color.White);
            DrawFadingMessage(batch, "January 16, 2011", 362, 400, 5000, msNow, 6000, Color.White);


            //DrawFadingMessage(batch, "You were slain by a flying corpse,", 20, color);
            //DrawFadingMessage(batch, "borne aloft on the wings of your", 60, color);
            //DrawFadingMessage(batch, "revolving door.", 100, color);

            //DrawFadingMessage(batch, "Press B to try again.", 180, color);

            //DrawCentered(batch, "- game over -", 360, Color.Red);
        }

    }
}
