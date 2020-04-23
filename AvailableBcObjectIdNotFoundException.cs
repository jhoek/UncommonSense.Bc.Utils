using System;

namespace UncommonSense.Bc.Utils
{
    [Serializable()]
    public class AvailableBcObjectIdNotFoundException : Exception
    {
        public AvailableBcObjectIdNotFoundException(ObjectType objectType, int quantity) :
            base($"Could not find {quantity} available {objectType} IDs.")
        {
            ObjectType = objectType;
            Quantity = quantity;
        }

        public ObjectType ObjectType { get; }
        public int Quantity { get; }
    }
}