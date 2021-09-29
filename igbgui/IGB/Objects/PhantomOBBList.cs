using igbgui.Types;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public class PhantomOBBList : igObjectList<PhantomOBB>, IigList<PhantomOBB>
    {
        public PhantomOBBList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }
    }
}
