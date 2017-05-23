namespace Weka
{
    public struct Property
    {
        public Property(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => Value;
    }
}