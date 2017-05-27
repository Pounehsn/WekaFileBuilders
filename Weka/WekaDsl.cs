using System.Collections.Generic;
using System.Linq;
using Core;

namespace Weka
{
    public static class WekaDsl
    {
        public static WekaTypeBase Nom(string arg0, params string[] args)
        {
            var nominal = new Nominal();
            nominal.Values.Add(new Id(arg0));
            foreach (var result in args.Select(i => new Id(i)))
            {
                nominal.Values.Add(result);
            }
            return nominal;
        }

        public static WekaTypeBase Num => Numeric.Instance;
        public static Instance Inst(params string[] args)
        {
            var instance = new Instance();
            instance.Properties.AddRange(args.Select(i => new Property(i)));
            return instance;
        }
        public static WekaAttribute Attr(string name, WekaTypeBase type) =>
            new WekaAttribute
            {
                Name = new Name(name),
                Type = type
            };

        public static IEnumerable<WekaAttribute> Attributes(params WekaAttribute[] args) => args;
        public static IEnumerable<Instance> Instances(params Instance[] args) => args;
        public static WekaInputFileBuilder WekaFile(
            string relation, 
            IEnumerable<WekaAttribute> attributes,
            IEnumerable<Instance> instances)
        {
            var file = new WekaInputFileBuilder {Relation = new Name(relation)};
            file.Attributes.AddRange(attributes);
            file.Data.AddRange(instances);

            return file;
        }
    }
}
