using System.Collections.Generic;

namespace Publications.BusinessLogic
{
    public interface IRelationship<out TDestination> : IEnumerable<TDestination>
    {
        int Count { get; }
    }
}