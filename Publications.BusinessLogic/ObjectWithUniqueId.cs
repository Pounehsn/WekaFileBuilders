using System;
using System.Collections.Generic;
using Core;

namespace Publications.BusinessLogic
{
    public abstract class ObjectWithUniqueId : IEquatable<ObjectWithUniqueId>
    {
        protected ObjectWithUniqueId(Id id)
        {
            if (IdToIndexMap.ContainsKey(id))
                throw new DuplicatedIdException($"Duplictated {GetType().Name} id '{id}' exception.");

            _uniqueId = IdToIndexMap.Count;
            IdToIndexMap.Add(id, this);
        }

        public abstract ObjectWithUniqueId Create(Id id);

        protected static readonly Dictionary<Id, ObjectWithUniqueId> IdToIndexMap = new Dictionary<Id, ObjectWithUniqueId>();
        private readonly int _uniqueId;

        public override bool Equals(object obj) => (obj as ObjectWithUniqueId)?._uniqueId == _uniqueId;

        public bool Equals(ObjectWithUniqueId other) => other?._uniqueId == _uniqueId;

        public override int GetHashCode() => _uniqueId;
    }
}
