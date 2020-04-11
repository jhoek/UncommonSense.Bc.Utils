using System.Collections.ObjectModel;
using System.Linq;

namespace UncommonSense.Bc.Utils
{
    public class ObjectIdAvailabilitySummaries : KeyedCollection<int, ObjectIdAvailabilitySummary>
    {
        public ObjectIdAvailabilitySummaries(ObjectType[] objectTypes)
        {
            ObjectTypes = objectTypes;
        }

        public ObjectType[] ObjectTypes { get; }

        protected override int GetKeyForItem(ObjectIdAvailabilitySummary item) => item.ID;

        public bool AddIdRange(ObjectIdRange objectIdRange)
        {
            // Returns false if any of the IDs in the range are already present in the collection

            if (!ObjectTypes.Contains(objectIdRange.ObjectType))
                return true;

            var result = true;

            foreach (var objectID in objectIdRange.IDs)
            {
                if (!this.Contains(objectID))
                {
                    Add(new ObjectIdAvailabilitySummary(objectIdRange.ObjectType, objectID, ObjectTypes));
                }
                else
                {
                    if (this[objectID].GetAvailability(objectIdRange.ObjectType) == Availability.Available)
                        result = false;
                    else
                        this[objectID].SetAvailability(objectIdRange.ObjectType, Availability.Available);
                }
            }

            return result;
        }

        public bool AddReservedObject(ObjectIdInfo objectIdInfo)
        {
            // Returns false if the object's ID is outside of the ranges in the collection
            if (!ObjectTypes.Contains(objectIdInfo.ObjectType))
                return true;

            if (Contains(objectIdInfo.ObjectID))
            {
                this[objectIdInfo.ObjectID].Properties[objectIdInfo.ObjectType.ToString()].Value = Availability.Reserved;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddUsedObject(ObjectIdInfo objectIdInfo)
        {
            // Returns false if the object's ID is outside of the ranges in the collection

            if (!ObjectTypes.Contains(objectIdInfo.ObjectType))
                return true;

            if (Contains(objectIdInfo.ObjectID))
            {
                this[objectIdInfo.ObjectID].Properties[objectIdInfo.ObjectType.ToString()].Value = Availability.InUse;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}