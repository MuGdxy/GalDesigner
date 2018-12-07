﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class BehaviorSystem
    {
        public RequireComponents RequireComponents { get; set; }
        public string Name { get; }
        public bool IsActive { get; set; }

        public BehaviorSystem(string name)
        {
            RequireComponents = new RequireComponents();
            Name = name;
            IsActive = true;
        }

        protected internal abstract void Excute(GameObject gameObject);
        
    }
}
