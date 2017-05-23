using Core;

namespace Weka
{
    public class WekaAttribute
    {
        public Name Name { get; set; }
        public WekaTypeBase Type { get; set; }

        public override string ToString() => $"{Name} {Type}";
    }
}