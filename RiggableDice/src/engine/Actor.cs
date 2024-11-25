using System;
using System.Collections.Generic;
using System.Linq;
using MathLibrary;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RiggableDice
{
    internal class Actor
    {
        private Component[] _componentsToRemove = new Component[0];

        private Transform2D _transform;
        private bool _started = false;
        private bool _enabled = true;

        private Component[] _components;



        public string Name { get; set; }
        public bool Started { get => _started; }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                // If enabled would not be changed, do nothing
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
        public Transform2D Transform { get; protected set; }

        public Actor(string name = "Actor")
        {
            Name = name;
            Transform = new Transform2D(this);
            _components = new Component[0];
            Start();
        }

        public static Actor Instantiate(
            Actor actor,
            Transform2D parent = null,
            Vector2 position = new Vector2(), 
            float rotation = 0)
        {


            // set actor's transform values
            actor.Transform.LocalPosition = position;
            actor.Transform.Rotate(rotation);
            if (parent != null)
                parent.AddChild(actor.Transform);



            // Add actor to current scene
            Game.CurrentScene.AddActor(actor);

            return actor;
        }

        public static void Destroy(Actor actor)
        {
            // remove children
            foreach (Transform2D child in actor.Transform.Children)
            {
                actor.Transform.RemoveChild(child);
            }

            // unchild from parent
            if (actor.Transform.Parent != null)
                actor.Transform.Parent.RemoveChild(actor.Transform);

            actor.End();

            // remove the actor from the current scene
            Game.CurrentScene.RemoveActor(actor);
        }

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void Start()
        {
            _started = true;
        }

        public virtual void Update(double deltaTime)
        {
            // Update all components
            foreach (Component component in _components)
            {
                if (!component.Started)
                    component.Start();

                component.Update(deltaTime);
            }

            RemoveComponentsToBeRemoved();
        }

        public virtual void End()
        {
            // end all of the actor's components
            foreach (Component component in _components)
            {
                component.End();
            }
        }

        // add component
        public T AddComponent<T>(T component) where T : Component
        {
            // Create temporary array one bigger than _components
            Component[] temp = new Component[_components.Length + 1];

            // copy over everything in _components into temp
            for (int i = 0; i < _components.Length; i++)
            {
                temp[i] = _components[i];
            }

            // set the last index in temp to the component we wanna add
            temp[temp.Length - 1] = component;

            // store temp in _components
            _components = temp;

            return component;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            component.Owner = this;

            return AddComponent(component);
        }

        // remove component
        public bool RemoveComponent<T>(T component) where T : Component
        {
            // if there are no components to remove, don't do anything and leave
            if (_components.Length == 0)
                return false;

            // if there is only one component and it is the one we're looking for, remove it
            if (_components.Length == 1 && _components[0] == component)
            {
                AddComponentToRemove(component);
                return true;
            }

            // loop thorugh components
            foreach (Component comp in _components)
            {
                // if the component is what we're removing
                if (comp == component)
                {
                    AddComponentToRemove(comp);

                    return true;
                }
            }
            return false;
        }

        public bool RemoveComponent<T>() where T : Component
        {
            // remove the component if it is of the type provided
            T component = GetComponent<T>();
            if (component != null)
                return RemoveComponent(component);
            return false;
        }

        // get component
        public T GetComponent<T>() where T: Component
        {
            // return the component if it is of the type provided
            foreach (Component component in _components)
            {
                if (component is T)
                    return (T)component;
            }
            return null;
        }
        // get more than one component
        public T[] GetComponents<T>() where T : Component
        {
            // create an temp array of the same size as _components
            T[] temp = new T[_components.Length];

            // copy everything that is T into temp
            int count = 0;


            for (int i = 0; i < _components.Length; i++)
            {
                if (_components[i] == (T)_components[i])
                {
                    temp[count] = (T)_components[i];
                    count++;
                }

            }

            // trim the array
            T[] result = new T[count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = temp[i];
            }

            return result;
        }

        private void AddComponentToRemove(Component component)
        {
            // make sure the component isnt already being removed
            foreach (Component comp in _componentsToRemove)
            {
                if (comp == component)
                    return;
            }

            // add that component
            Component[] temp = new Component[_componentsToRemove.Length + 1];

            // copy over everything in _components into temp
            for (int i = 0; i < _componentsToRemove.Length; i++)
            {
                temp[i] = _componentsToRemove[i];
            }

            // set the last index in temp to the component we wanna remove
            temp[temp.Length - 1] = component;

            // store temp in _componentsToRemove
            _componentsToRemove = temp;
        }

        private void RemoveComponentsToBeRemoved()
        {
            if (_componentsToRemove.Length <= 0)
                return;

            // create temp array for _components
            Component[] tempComponents = new Component[_components.Length];

            // deep copy the array, removing the elements in components to remove
            int j = 0;
            for (int i = 0; i < _components.Length; i++)
            {
                bool removed = false;
                foreach (Component component in _componentsToRemove)
                {
                    if (_components[i] == component)
                    {
                        removed = true;
                        component.End();
                        break;
                    }
                }

                if (!removed)
                {
                    tempComponents[j] = _components[i];
                    j++;
                }
            }

            // trim the array
            Component[] result = new Component[_components.Length - _componentsToRemove.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = tempComponents[i];
            }

            // set the array, clear out _componentsToRemove[] and go home
            _components = result;
            _componentsToRemove = new Component[0];
        }
    }
}
