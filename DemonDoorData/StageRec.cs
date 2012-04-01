using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace DemonDoor
{
    static class VectorParser
    {
    }

    public class CorpseRec
    {

    }

    public class GunRec
    {
        public string R0 { set { _r0 = Vector2.Zero; } }
        private Vector2 _r0;

        public string Size { set { _size = Vector2.Zero; } }
        private Vector2 _size;
    }

    public class StageRec
    {
        public string Title;
        public GunRec[] Guns;
    }
}
