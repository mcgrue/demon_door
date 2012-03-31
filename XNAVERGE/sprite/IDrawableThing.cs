using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAVERGE {
    
    public interface IDrawableThing {
        int GetX();
        int GetY();
        RenderDelegate GetDrawDelegate();
    }

    /*
    public delegate void CollidableDelegate( ICollidableThing thing_that_i_hit );

    interface ICollidableThing {

    }
     * */
}
