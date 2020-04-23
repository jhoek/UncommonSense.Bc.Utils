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
            // FIXME: Other object types
        }

        private const string AvailableBcObjectIdNotFoundErrorID = "UncommonSense.Bc.Utils.AvailableBcObjectIdNotFound";

        protected void FindObjectIds(ObjectType objectType, int quantity)
        {
            if (quantity > 0)
                if (!FindSequentialObjectIds(objectType, quantity))
                    if (!FindIndividualObjectIds(objectType, quantity))
                        WriteError(
                            new ErrorRecord(
                                new AvailableBcObjectIdNotFoundException(objectType, quantity),
                                AvailableBcObjectIdNotFoundErrorID,
                                ErrorCategory.ObjectNotFound,
                                null
                            )
                        );
        }

        protected bool FindSequentialObjectIds(ObjectType objectType, int quantity)
        {
            var sequence = cache
                .Where(c => c.ObjectType == objectType)
                .Select((c, i) => new { ObjectID = c.ObjectID, SequenceId = c.ObjectID - i })
                .GroupBy(c => c.SequenceId)
                .Where(g => g.Count() >= quantity)
                .FirstOrDefault();

            if (sequence == null)
            {
                WriteWarning($"Could not find {quantity} available, sequential {objectType} IDs.");
                return false;
            }

            sequence
                .Take(quantity)
                .Select(c => new ObjectIdInfo(objectType, c.ObjectID))
                .ForEach(i => WriteObject(i));

            return true;
        }

        protected bool FindIndividualObjectIds(ObjectType objectType, int quantity)
        {
            var ids = cache.Where(c => c.ObjectType == objectType);

            if (ids.Count() >= quantity)
            {
                ids
                    .Take(quantity)
                    .Select(i => new ObjectIdInfo(objectType, i.ObjectID))
                    .ForEach(i => WriteObject(i));

                return true;
            }

            return false;
        }
    }
}