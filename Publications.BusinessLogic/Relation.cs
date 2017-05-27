using System.Collections;
using System.Collections.Generic;

namespace Publications.BusinessLogic
{
    public class Relationship<TDestination> : IEnumerable<TDestination>
    {
        private readonly HashSet<TDestination> _relations = new HashSet<TDestination>();
        public virtual bool Add(TDestination destinantionObject) => _relations.Add(destinantionObject);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<TDestination> GetEnumerator() => _relations.GetEnumerator();
    }
}
