using System.Collections.Generic;
using Core;

namespace Weka
{
    public class Nominal : WekaTypeBase
    {
        public Nominal()
        {
            Values = new List<Id>();
        }

        public List<Id> Values { get; }
        public override string ToString() => $"{{{string.Join(", ", Values)}}}";
    }
}