using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAVERGE
{
    public interface IBrainyThing
    {
        void ProcessBehavior(GameTime time);
    }
}
