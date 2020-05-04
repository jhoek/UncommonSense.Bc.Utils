using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UncommonSense.Bc.Utils
{
    public class ObjectIdInfo
    {
        internal ObjectIdInfo(ObjectType objectType, int objectID)
        {
            ObjectType = objectType;
            ObjectID = objectID;
        }

        public override string ToString() => $"{ObjectType} {ObjectID}";

        public ObjectType ObjectType { get; }
        public int ObjectID { get; }
    }
}
