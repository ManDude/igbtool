using igbgui.Types;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public class PhantomAABBList : igObjectList<PhantomAABB>, IigList<PhantomAABB>
    {
        public PhantomAABBList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }
    }
}
