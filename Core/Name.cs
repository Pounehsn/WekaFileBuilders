namespace Core
{
    public struct Name
    {
        public Name(string name)
        {
            this.Value = name;
        }

        public string Value { get; }

        public override string ToString() => Value;
    }
}