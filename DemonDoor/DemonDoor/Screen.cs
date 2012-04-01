using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DemonDoor
{
    abstract class Screen
    {
        internal abstract void Load();

        internal abstract void Update(GameTime gameTime);

        internal abstract void Draw(SpriteBatch batch, GameTime gameTime);
    }
}
