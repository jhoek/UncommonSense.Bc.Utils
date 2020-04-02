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
        internal ObjectIdInfo(ObjectType type, int id)
        {
            Type = type;
            ID = id;
        }

        public ObjectType Type { get; }
        public int ID { get; }
    }
}