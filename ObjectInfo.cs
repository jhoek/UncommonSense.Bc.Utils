namespace UncommonSense.Bc.Utils
{
    public class ObjectInfo : ObjectIdInfo
    {
        internal ObjectInfo(ObjectType type, int id, string name, string baseName) : base(type, id)
        {
            Name = name;
            BaseName = baseName;
        }

        public string Name { get; }
        public string BaseName { get; }
    }
}