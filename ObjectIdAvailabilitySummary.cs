using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    public class ObjectIdAvailabilitySummary : PSObject
    {
        internal ObjectIdAvailabilitySummary(ObjectType type, int id, ObjectType[] relevantObjectTypes)
        {
            Properties.Add(new PSNoteProperty("ID", id));
            relevantObjectTypes.ForEach(t => SetAvailability(t, t == type ? Availability.Available : Availability.NotInRange));
        }

        public int ID => (int)Properties["ID"].Value;

        public Availability? GetAvailability(ObjectType objectType) =>
            Properties.Any(p => p.Name == objectType.ToString()) ?
                (Availability)Properties[objectType.ToString()].Value :
                default(Availability?);

        public void SetAvailability(ObjectType objectType, Availability availability)
        {
            if (GetAvailability(objectType).HasValue)
                Properties[objectType.ToString()].Value = availability;
            else
                Properties.Add(new PSNoteProperty(objectType.ToString(), availability));
        }

    }
}