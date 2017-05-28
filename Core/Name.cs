namespace Core
{
    public struct Name
    {
        public Name(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => Value;

        public override bool Equals(object other) =>
            other is Name && Equals((Name)other);

        public bool Equals(Name other) => Value == other.Value;

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
    }
}