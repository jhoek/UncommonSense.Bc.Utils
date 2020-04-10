using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    public class ObjectIdAvailabilitySummary : PSObject
    {
        internal ObjectIdAvailabilitySummary(int id)
        {
            ID = id;
        }

        public int ID { get; }

        public void SetAvailability(ObjectType objectType, Availability availability)
        {
            var propertyName = objectType.ToString();

            if (Properties[propertyName] != null)
                Properties.Remove(propertyName);

            Properties.Add(new PSNoteProperty(propertyName, availability));
        }
    }
}