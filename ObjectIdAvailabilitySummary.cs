using System;
using System.Collections.Generic;
using System.Linq;

namespace UncommonSense.Bc.Utils
{
    public class ObjectIdAvailabilitySummary
    {
        private Dictionary<int, ObjectIdAvailabilitySummaryItem> innerDictionary = new Dictionary<int, ObjectIdAvailabilitySummaryItem>();

        public ObjectIdAvailabilitySummary(ObjectType[] consideredObjectTypes)
        {
            ConsideredObjectTypes = consideredObjectTypes ?? Helper.AllObjectTypes().ToArray();
        }

        public ObjectType[] ConsideredObjectTypes { get; }

        public IEnumerable<ObjectIdAvailabilitySummaryItem> Items => innerDictionary.Values;

        public bool AddIdRange(ObjectIdRange objectIdRange)
        {
            var result = true; // Assume no overlapping ranges

            // Test for relevant object types
            if (!ConsideredObjectTypes.Contains(objectIdRange.ObjectType))
                return result;

            foreach (var objectID in objectIdRange.IDs)
            {
                if (!innerDictionary.ContainsKey(objectID))
                {
                    innerDictionary.Add(objectID, new ObjectIdAvailabilitySummaryItem(objectID).SetAvailability(objectIdRange.ObjectType, Availability.Available));
                }
                else
                {
                    if (innerDictionary[objectID].GetAvailability(objectIdRange.ObjectType) == Availability.Available)
                        result = false;
                    else
                        innerDictionary[objectID].SetAvailability(objectIdRange.ObjectType, Availability.Available);
                }
            }

            return result;
        }

        public bool AddReservedObject(ObjectIdInfo objectIdInfo)
        {
            if (!ConsideredObjectTypes.Contains(objectIdInfo.ObjectType))
                return true;

            if (innerDictionary.ContainsKey(objectIdInfo.ObjectID))
            {
                innerDictionary[objectIdInfo.ObjectID].SetAvailability(objectIdInfo.ObjectType, Availability.Reserved);
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

            if (!ConsideredObjectTypes.Contains(objectIdInfo.ObjectType))
                return true;

            if (innerDictionary.ContainsKey(objectIdInfo.ObjectID))
            {
                innerDictionary[objectIdInfo.ObjectID].SetAvailability(objectIdInfo.ObjectType, Availability.InUse);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}