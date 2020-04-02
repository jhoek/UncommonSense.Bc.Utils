using UncommonSense.Bc.Utils;

namespace UncommonSense.Bc.Utils
{
    public enum ObjectType
    {
        Table,
        TableExtension,
        Page,
        PageExtension,
        PageCustomization,
        Report,
        XmlPort,
        Query,
        Codeunit,
        Profile,
        Enum,
        EnumExtension
    }

    public class ObjectIdInfo
    {
        public ObjectIdInfo(ObjectType type, int id)
        {
            Type = type;
            ID = id;
        }

        public ObjectType Type { get; }
        public int ID { get; }
    }

    public class ObjectInfo : ObjectIdInfo
    {
        public ObjectInfo(ObjectType type, int id, string name, string baseName) : base(type, id)
        {
            Name = name;
            BaseName = baseName;
        }

        public string Name { get; }
        public string BaseName { get; }
    }
}