using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathLibrary;

namespace RiggableDice
{
    internal class Transform2D
    {
        private Matrix3 _localMatrix = Matrix3.Identity;
        private Matrix3 _globalMatrix = Matrix3.Identity;

        private Matrix3 _localTranslation = Matrix3.Identity;
        private Matrix3 _localRotation = Matrix3.Identity;
        private Matrix3 _localScale = Matrix3.Identity;

        private Actor _owner;

        private Transform2D _parent;
        private Transform2D[] _children;

        private float _localRotationAngle;

        public Matrix3 LocalRotation
        {
            get { return _localRotation; }

            set
            {
                _localRotation = value;
                _localRotationAngle = -(float)Math.Atan2(_localRotation.m01, _localRotation.m00);

                UpdateTransforms();
            }
        }

        public Vector2 LocalPosition
        {
            get { return new Vector2(_localTranslation.m02, _localTranslation.m12); }

            set
            {
                _localTranslation.m02 = value.x;
                _localTranslation.m12 = value.y;

                UpdateTransforms();
            }
        }

        public Vector2 GlobalPosition
        {
            get { return new Vector2(_globalMatrix.m02, _globalMatrix.m12); }
        }

        public Vector2 LocalScale
        {
            get { return new Vector2(_localScale.m00, _localScale.m11); }

            set
            {
                _localScale.m00 = value.x;
                _localScale.m11 = value.y;

                UpdateTransforms();
            }
        }

        public Vector2 GlobalScale
        {
            get
            {
                Vector2 xAxis = new Vector2(_globalMatrix.m00, _globalMatrix.m10);
                Vector2 yAxis = new Vector2(_globalMatrix.m01, _globalMatrix.m11);

                return new Vector2(xAxis.Magnitude, yAxis.Magnitude);
            }
        }

        public Actor Owner
        {
            get { return _owner; }
        }

        public Vector2 Forward
        {
            get { return new Vector2(_globalMatrix.m00, _globalMatrix.m10).Normalized; }
        }

        public Vector2 Right
        {
            get { return new Vector2(_globalMatrix.m10, _globalMatrix.m11).Normalized; }
        }

        public float LocalRotationAngle
        {
            get { return _localRotationAngle; }
        }

        public float GlobalRotationAngle
        {
            get { return (float)Math.Atan2(_globalMatrix.m01, _globalMatrix.m00); }
        }

        public Transform2D Parent { get => _parent; }

        public Transform2D[] Children { get => _children; }

        // waogh thats a lotta variables up there !  good thing theres no more code !
        // hahaha. just kiddin. more code

        public Transform2D(Actor owner)
        {
            _owner = owner;
            _children = new Transform2D[0];
        }

        public void Translate(Vector2 direction)
        {
            LocalPosition += direction;
        }

        public void Translate(float x, float y)
        {
            LocalPosition += new Vector2(x, y);
        }

        public void Rotate(float radians)
        {
            LocalRotation = Matrix3.CreateRotation(_localRotationAngle + radians);
        }

        public void AddChild(Transform2D child)
        {
            // do not add the child if it is this transform's parent
            if (child == _parent)
            {
                return;
            }

            // create a temporary array to copy  everything over and also add the specificed child to
            // then copy it over and set the child's parent to this transform
            Transform2D[] tempArray = new Transform2D[_children.Length + 1];
            for (int i = 0; i < _children.Length; i++)
            {
                tempArray[i] = _children[i];
            }
            tempArray[tempArray.Length - 1] = child;
            child._parent = this;
            _children = tempArray;
            child.UpdateTransforms();
        }

        public bool RemoveChild(Transform2D child)
        {
            bool childRemoved = false;

            // if there are no children to remove, don't do anything and leave
            if (_children.Length == 0)
            {
                return false;
            }

            // create a temporary array to copy everything over and remove the specified child from
            // then copy it over to the children array if the child was successfully removed
            Transform2D[] tempArray = new Transform2D[_children.Length - 1];

            int j = 0;

            for (int i = 0; i < _children.Length; i++)
            {
                if (_children[i] != child)
                {
                    tempArray[j] = _children[i];
                    j++;
                }
                else
                {
                    childRemoved = true;
                }
            }

            if (childRemoved)
            {
            _children = tempArray;
            child._parent = null;
            }

            return childRemoved;
        }

        public void UpdateTransforms()
        {
            _localMatrix = _localTranslation * _localRotation * _localScale;
            
            if (_parent != null)
            {
                _globalMatrix = _parent._globalMatrix * _localMatrix;
            }
            else
            {
                _globalMatrix = _localMatrix;
            }
            
            // update the transforms of every child
            foreach (Transform2D child in _children)
            {
                child.UpdateTransforms();
            }
        }

        public string GlobalMatrixToString()
        {
            return _globalMatrix.ToString();
        }

        public string LocalMatrixToString()
        {
            return _localMatrix.ToString();
        }
    }
}
