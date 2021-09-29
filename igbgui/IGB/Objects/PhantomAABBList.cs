using igbgui.Fields;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace igbgui.Objects
{
    public class PhantomAABBList : igObjectList<PhantomAABB>, IigList<PhantomAABB>
    {
        public PhantomAABBList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }
    }
}
