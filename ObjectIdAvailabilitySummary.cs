namespace UncommonSense.Bc.Utils
{
    public class ObjectIdAvailabilitySummary
    {
        internal ObjectIdAvailabilitySummary(int id)
        {
            ID = id;
        }

        public int ID { get; }
        public Availability Table { get; set; }
        public Availability Page { get; set; }

        // FIXME: Rest
    }
}
