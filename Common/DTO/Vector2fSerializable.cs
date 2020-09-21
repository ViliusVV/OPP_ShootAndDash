using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    [Serializable()]
    public class Vector2fSerializable

    {
        [NonSerialized()]
        private Vector2f _vector;

        public float X 
        { 
            get => _vector.X;
            set => _vector.X = value;
        }

        public float Y
        {
            get => _vector.Y;
            set => _vector.Y = value;
        }

        public Vector2fSerializable() { }

        public Vector2fSerializable(Vector2f vector)
        {
            this._vector = vector;
        }

        public static implicit operator Vector2f(Vector2fSerializable v) => v._vector;
        public static implicit operator Vector2fSerializable(Vector2f v) => new Vector2fSerializable(v);

        public override string ToString()
        {
            return _vector.ToString();
        }
    }
}
