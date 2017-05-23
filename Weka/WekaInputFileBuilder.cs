using System;
using System.Collections.Generic;
using Core;

namespace Weka
{
    public class WekaInputFileBuilder
    {
        public WekaInputFileBuilder()
        {
            Attributes = new List<WekaAttribute>();
            Data = new List<Instance>();
        }

        public Name Relation { get; set; }
        public List<WekaAttribute> Attributes { get; }
        public List<Instance> Data { get; }

        public IEnumerable<string> GetFileLines()
        {
            yield return $"@relation {Relation.Value}";

            yield return string.Empty;

            foreach (var attribute in Attributes)
            {
                yield return $"@attribute {attribute}";
            }

            yield return string.Empty;
            yield return "@data";
            foreach (var instance in Data)
            {
                yield return instance.ToString();
            }
        }

        public override string ToString() => string.Join(Environment.NewLine, GetFileLines());
    }
}
