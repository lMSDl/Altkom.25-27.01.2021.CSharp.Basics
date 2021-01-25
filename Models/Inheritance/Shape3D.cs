﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Inheritance
{
    public abstract class Shape3D : Shape, IShapeArea
    {
        public abstract float Volume { get; }
        public abstract float Area { get; }


    }
}
