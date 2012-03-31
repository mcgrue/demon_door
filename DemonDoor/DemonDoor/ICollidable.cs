using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemonDoor
{
    interface ICollidable
    {
        void Collided(ICollidable other);
    }
}
