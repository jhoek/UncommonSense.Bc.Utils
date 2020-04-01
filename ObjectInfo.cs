using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UncommonSense.Bc.Utils
{
    public class ObjectInfo
    {
        public static readonly Regex pattern = new Regex(@"^(?<Type>[A-Za-z]+)\s+(?<ID>\d+)\s+(?<Name>.*?)(\s+extends\s+(?<BaseName>.*))?$");

        public static IEnumerable<ObjectInfo> Create(string signature)
        {
            switch (pattern.Match(signature))
            {
                case Match m when m.Success:
                    yield return new ObjectInfo()
                    {
                        TypeText = m.Groups["Type"].Value,
                        Type = ParseObjectType(m.Groups["Type"].Value),
                        ID = int.Parse(m.Groups["ID"].Value),
                        Name = m.Groups["Name"].Value,
                        BaseName = m.Groups["BaseName"].Value
                    };

                    break;

                default:
                    yield break;
            }
        }

        public static ObjectType ParseObjectType(string typeText)
        {
            var result = ObjectType.Unknown;
            Enum.TryParse<ObjectType>(typeText, true, out result);
            return result;
        }

        protected ObjectInfo()
        {
        }

        public override string ToString() => $"{(Type == ObjectType.Unknown ? TypeText : Type.ToString())} {ID} {Name}";

        public string TypeText { get; protected set; }
        public ObjectType Type { get; protected set; }
        public int ID { get; protected set; }
        public string Name { get; protected set; }
        public string BaseName { get; protected set; }
    }
}
