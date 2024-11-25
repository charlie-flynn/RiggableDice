using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiggableDice
{
    internal class Component
    {
        private Actor _owner;
        private bool _started;
        private bool _enabled;
        public bool Started { get => _started; }
        public Actor Owner { get => _owner; set => _owner = value; }

        public Component(Actor owner)
        {
            _owner = owner;
            _enabled = true;
            _started = false;
        }
        public bool Enabled
        {
            get => _enabled;
            set
            {
                // If enabled would not change, do nothing
                if (_enabled == value) return;

                _enabled = value;
                // if value is true, call OnEnable
                if (_enabled)
                    OnEnable();
                // if value is false, call OnDisable
                else
                    OnDisable();
            }
        }

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void Start() 
        {
            _started = true;
        }
        public virtual void Update(double deltaTime) 
        {
            if (!Enabled && !Started)
                return;
        }
        public virtual void End() { }
    }
}
