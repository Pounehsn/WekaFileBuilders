using System;
using System.Collections.Generic;
using Core;

namespace Publications.BusinessLogic
{
    public abstract class ObjectWithUniqueId
    {
        protected static readonly HashSet<Action> Resets = new HashSet<Action>();

        public static void ResetAll()
        {
            foreach (var r in Resets)
            {
                r();
            }
        }
    }

    public abstract class ObjectWithUniqueId<TObject> : ObjectWithUniqueId, IEquatable<TObject>
        where TObject : ObjectWithUniqueId<TObject>
    {
        protected ObjectWithUniqueId(Id id)
        {
            if (Objects.ContainsKey(id))
                throw new DuplicatedIdException($"Duplictated {GetType().Name} id '{id}' exception.");

            _uniqueId = Objects.Count;
            Objects.Add(id, (TObject)this);

            Id = id;
        }

        static ObjectWithUniqueId()
        {
            Resets.Add(() => Objects.Clear());
        }

        private static readonly Dictionary<Id, TObject> Objects
            = new Dictionary<Id, TObject>();
        private readonly int _uniqueId;

        public static TObject Get(Id id) => Objects[id];

        public static bool TryGet(Id id, out TObject o)
        {
            if (Objects.ContainsKey(id))
            {
                o = Objects[id];
                return true;
            }

            o = null;
            return false;
        }

        public static TObject GetOrCreateInstance(Id id, Func<Id, TObject> factory) =>
            Objects.ContainsKey(id) ? Objects[id] : factory(id);

        public static IEnumerable<TObject> GetAll() => Objects.Values;

        public Id Id { get; }

        public int UniqueId => _uniqueId;

        public override bool Equals(object obj) => (obj as TObject)?._uniqueId == _uniqueId;

        public bool Equals(TObject other) => other?._uniqueId == _uniqueId;

        public override int GetHashCode() => _uniqueId;

        public override string ToString() => Id.ToString();
    }
}
