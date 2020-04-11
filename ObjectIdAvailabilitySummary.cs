using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    public class ObjectIdAvailabilitySummary : PSObject // FIXME: Doesn't seem to add much; consider Object as base type
    {
        internal ObjectIdAvailabilitySummary(ObjectType objectType, int objectID, ObjectType[] relevantObjectTypes)
        {
            Properties.Add(new PSNoteProperty(nameof(ObjectID), objectID));
            TypeNames.Insert(0, "UncommonSense.Bc.Utils.ObjectIdAvailabilitySummary");
            relevantObjectTypes.ForEach(t => SetAvailability(t, t == objectType ? Availability.Available : Availability.NotInRange));
        }

        public int ObjectID => (int)(Properties[nameof(ObjectID)]?.Value ?? 0);

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