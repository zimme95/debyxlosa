﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Util.Observer;

namespace GameEngine.Components
{
    public abstract class EntityComponent : SystemObservable
    {
        private int _entityId;
        public EntityComponent() { }
        public EntityComponent(int Id)
        {
            _entityId = Id;
        }

        public int EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }
    }
}
