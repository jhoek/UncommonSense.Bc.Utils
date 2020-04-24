using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils.Cmdlets
{
    [Cmdlet(VerbsCommon.Find, "AvailableBcObjectId")]
    [OutputType(typeof(ObjectIdInfo))]
    public class FindAvailableBcObjectIdCmdlet : Cmdlet
    {
        private List<ObjectIdAvailability> availableIdCache;

        // FIXME: Consider making ObjectIdAvailableity a scriptblock - would require addiotnal parameters to pass to scriptblock
        // FIXME: as $objectType, pass it only the requested types, e.g. -Table 4 => -ObjectType Table

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public ObjectIdAvailability[] ObjectIdAvailability { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int Table { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int TableExtension { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int Page { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int PageExtension { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int PageCustomization { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int Report { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int Codeunit { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int XmlPort { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int Query { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int Profile { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int Enum { get; set; }

        [Parameter()]
        [ValidateRange(1, int.MaxValue)]
        public int EnumExtension { get; set; }

        [Parameter()]
        public SwitchParameter Contiguous { get; set; }

        protected override void BeginProcessing() =>
            availableIdCache = new List<ObjectIdAvailability>();

        protected override void ProcessRecord() =>
            availableIdCache.AddRange(
                ObjectIdAvailability
                    .Where(a => a.Availability == Availability.Available)
            );

        protected override void EndProcessing()
        {
            FindObjectIds(ObjectType.Table, Table);
            FindObjectIds(ObjectType.TableExtension, TableExtension);
            FindObjectIds(ObjectType.Page, Page);
            FindObjectIds(ObjectType.PageExtension, PageExtension);
            FindObjectIds(ObjectType.PageCustomization, PageCustomization); // FIXME: Nodig?
            FindObjectIds(ObjectType.Report, Report);
            FindObjectIds(ObjectType.Codeunit, Codeunit);
            FindObjectIds(ObjectType.XmlPort, XmlPort);
            FindObjectIds(ObjectType.Query, Query);
            FindObjectIds(ObjectType.Profile, Profile);
            FindObjectIds(ObjectType.Enum, Enum);
            FindObjectIds(ObjectType.EnumExtension, EnumExtension);
        }

        private const string AvailableBcObjectIdNotFoundErrorID = "UncommonSense.Bc.Utils.AvailableBcObjectIdNotFound";

        protected void FindObjectIds(ObjectType objectType, int quantity)
        {
            if (quantity == 0)
                return;

            var success =
                Contiguous ?
                    FindSequentialObjectIds(objectType, quantity) :
                    FindIndividualObjectIds(objectType, quantity);

            if (!success)
            {
                WriteError(
                    new ErrorRecord(
                        new AvailableBcObjectIdNotFoundException(objectType, quantity, Contiguous),
                        AvailableBcObjectIdNotFoundErrorID,
                        ErrorCategory.ObjectNotFound,
                        null
                    )
                );
            }
        }

        protected bool FindSequentialObjectIds(ObjectType objectType, int quantity)
        {
            var sequence = availableIdCache
                .Where(c => c.ObjectType == objectType)
                .Select((c, i) => new { ObjectID = c.ObjectID, SequenceId = c.ObjectID - i })
                .GroupBy(c => c.SequenceId)
                .Where(g => g.Count() >= quantity)
                .FirstOrDefault();

            if (sequence != null)
            {
                sequence
                    .Take(quantity)
                    .Select(c => new ObjectIdInfo(objectType, c.ObjectID))
                    .ForEach(i => WriteObject(i));

                return true;
            }
            else
                return false;
        }

        protected bool FindIndividualObjectIds(ObjectType objectType, int quantity)
        {
            var ids = availableIdCache.Where(c => c.ObjectType == objectType);

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