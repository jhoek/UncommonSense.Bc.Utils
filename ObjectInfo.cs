namespace UncommonSense.Bc.Utils
{
    public class ObjectInfo : ObjectIdInfo
    {
        internal ObjectInfo(ObjectType objectType, int objectID, string objectName, string baseName) : base(objectType, objectID)
        {
            ObjectName = objectName;
            BaseName = baseName;
        }

        public string ObjectName { get; }
        public string BaseName { get; }
    }
}