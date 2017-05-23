using System;

namespace Core
{
    public struct Id
    {
        public Id(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => Value;
    }
}