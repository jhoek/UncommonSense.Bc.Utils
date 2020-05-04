using System;

namespace UncommonSense.Bc.Utils
{
    [Serializable()]
    public class AvailableBcObjectIdNotFoundException : Exception
    {
        public AvailableBcObjectIdNotFoundException(ObjectType objectType, int quantity, bool contiguous) :
            base($"Could not find{(contiguous ? " a contiguous range of " : " ")}{quantity} available {objectType} IDs.")
        {
            ObjectType = objectType;
            Quantity = quantity;
            Contiguous = contiguous;
        }

        public ObjectType ObjectType { get; }
        public int Quantity { get; }
        public bool Contiguous { get; }
    }
}