using System;
using System.Collections.Generic;
using System.Linq;

namespace UncommonSense.Bc.Utils
{
    public class ObjectIdRange
    {
        internal ObjectIdRange(ObjectType objectType, int fromObjectID, int toObjectID)
        {
            ObjectType = objectType;
            FromObjectID = Math.Min(fromObjectID, toObjectID);
            ToObjectID = Math.Max(fromObjectID, toObjectID);
        }

        public ObjectType ObjectType { get; }
        public int FromObjectID { get; }
        public int ToObjectID { get; }
        public int Count => ToObjectID - FromObjectID + 1;
        public IEnumerable<int> IDs => Enumerable.Range(FromObjectID, Count);
    }
}