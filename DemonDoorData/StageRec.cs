using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace DemonDoor
{
    public class CorpseRec
    {

    }

    public class GunRec
    {
        public Vector2 R0 { get; }
        public string R0 { set; }
        public Vector2 Size { get; }
        public string Size { set; }
    }

    public class StageRec
    {
        public string Title;
        public GunRec[] Guns;
    }
}
