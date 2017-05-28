using System;

namespace Core
{
    public struct Id : IEquatable<Id>
    {
        public Id(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => Value;

        public override bool Equals(object other) =>
            other is Id && Equals((Id)other);

        public bool Equals(Id other) => Value == other.Value;

        public override int GetHashCode() =>
            Value?.GetHashCode() ?? 0;
    }
}