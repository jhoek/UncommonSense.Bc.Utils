namespace UncommonSense.Bc.Utils
{
    public class ObjectIdAvailability
    {
        internal ObjectIdAvailability(ObjectType type, int id, Availability status)
        {
            Type = type;
            ID = id;
            Status = status;
        }

        public ObjectType Type { get; }
        public int ID { get; }
        public Availability Status { get; }
    }
}