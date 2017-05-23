using System.Collections.Generic;

namespace Weka
{
    public class Instance
    {
        public Instance()
        {
            Properties = new List<Property>();
        }

        public List<Property> Properties;

        public override string ToString() => string.Join(", ", Properties);
    }
}