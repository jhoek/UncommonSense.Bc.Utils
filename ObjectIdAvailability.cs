namespace UncommonSense.Bc.Utils
{
    public class ObjectIdAvailability
    {
        internal ObjectIdAvailability(ObjectType objectType, int objectID, Availability availability)
        {
            ObjectType = objectType;
            ObjectID = objectID;
            Availability = availability;
        }

        public ObjectType ObjectType { get; }
        public int ObjectID { get; }
        public Availability Availability { get; }

        public override string ToString() => $"{ObjectType} {ObjectID}: {Availability}";
    }
}