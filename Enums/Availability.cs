namespace UncommonSense.Bc.Utils
{
    public enum Availability
    {
        NotInRange, // Object is outside of given ID ranges
        Available, // Object is available
        Reserved, // Object is reserved
        InUse // Object is in use
    }
}