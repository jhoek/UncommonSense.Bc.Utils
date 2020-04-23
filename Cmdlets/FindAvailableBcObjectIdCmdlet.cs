using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils.Cmdlets
{
    [Cmdlet(VerbsCommon.Find, "AvailableBcObjectId")]
    [OutputType(typeof(ObjectIdInfo))]
    public class FindAvailableBcObjectIdCmdlet : Cmdlet
    {
        private List<ObjectIdAvailability> cache;

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public ObjectIdAvailability[] ObjectIdAvailability { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int Table { get; set; }

        protected override void BeginProcessing() =>
            cache = new List<ObjectIdAvailability>();

        protected override void ProcessRecord() =>
            cache.AddRange(ObjectIdAvailability.Where(a => a.Availability == Availability.Available));

        protected override void EndProcessing()
        {
            FindObjectIds(ObjectType.Table, Table);
        }

        protected void FindObjectIds(ObjectType objectType, int quantity)
        {
            if (quantity > 0)
            {
                var foundGroup = cache
                    .Where(c => c.ObjectType == objectType)
                    .Select((c, i) => new { ObjectID = c.ObjectID, GroupID = c.ObjectID - i })
                    .GroupBy(c => c.GroupID)
                    .Where(g => g.Count() >= quantity)
                    .FirstOrDefault();

                if (foundGroup == null)
                {
                    WriteWarning($"Could not find {quantity} available, sequential {objectType} IDs.");
                }
                else
                {
                    foundGroup
                        .Take(quantity)
                        .Select(c => new ObjectIdInfo(objectType, c.ObjectID));
                }
            }
        }
    }
}