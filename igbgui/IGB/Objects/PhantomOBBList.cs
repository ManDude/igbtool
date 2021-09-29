using igbgui.Fields;
using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace igbgui.Objects
{
    public class PhantomOBBList : igObjectList<PhantomOBB>, IigList<PhantomOBB>
    {
        public PhantomOBBList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }
    }
}
