using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UncommonSense.Bc.Utils
{
    public class ObjectIdInfo
    {
        internal ObjectIdInfo(ObjectType type, int id)
        {
            Type = type;
            ID = id;
        }

        public override string ToString() => $"{Type} {ID}";

        public ObjectType Type { get; }
        public int ID { get; }
    }
}
