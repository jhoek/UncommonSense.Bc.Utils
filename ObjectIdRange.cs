using System;

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
    }
}