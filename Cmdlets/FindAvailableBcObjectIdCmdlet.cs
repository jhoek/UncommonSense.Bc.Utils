using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils.Cmdlets
{
    [Cmdlet(VerbsCommon.Find, "AvailableBcObjectId")]
    [OutputType(typeof(ObjectIdInfo))]
    public class FindAvailableBcObjectIdCmdlet : Cmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty()]
        public string Path { get; set; } = ".";

        [Parameter()]
        public SwitchParameter Recurse { get; set; }

        [Parameter()]
        [ValidateNotNull()]
        public ScriptBlock ObjectIdAvailability { get; set; } = ScriptBlock.Create("param([string]$Path, [Switch]$Recurse, [UncommonSense.Bc.Utils.ObjectType[]]$ObjectType) Get-BcObjectIdAvailability -Path $Path -Recurse:$Recurse -ObjectType $ObjectType");

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

        protected override void EndProcessing()
        {
            FindObjectIds(ObjectType.Table, Table);
            FindObjectIds(ObjectType.TableExtension, TableExtension);
            FindObjectIds(ObjectType.Page, Page);
            FindObjectIds(ObjectType.PageExtension, PageExtension);
            FindObjectIds(ObjectType.Report, Report);
            FindObjectIds(ObjectType.Codeunit, Codeunit);
            FindObjectIds(ObjectType.XmlPort, XmlPort);
            FindObjectIds(ObjectType.Query, Query);
            FindObjectIds(ObjectType.Profile, Profile);
            FindObjectIds(ObjectType.Enum, Enum);
            FindObjectIds(ObjectType.EnumExtension, EnumExtension);
        }

        private const string AvailableBcObjectIdNotFoundErrorID = "UncommonSense.Bc.Utils.AvailableBcObjectIdNotFound";

        protected IEnumerable<ObjectType> RequestedObjectTypes
        {
            get
            {
                if (Table > 0) yield return ObjectType.Table;
                if (TableExtension > 0) yield return ObjectType.TableExtension;
                if (Page > 0) yield return ObjectType.Page;
                if (PageExtension > 0) yield return ObjectType.PageExtension;
                if (Report > 0) yield return ObjectType.Report;
                if (Codeunit > 0) yield return ObjectType.Codeunit;
                if (XmlPort > 0) yield return ObjectType.XmlPort;
                if (Query > 0) yield return ObjectType.Query;
                if (Profile > 0) yield return ObjectType.Profile;
                if (Enum > 0) yield return ObjectType.Enum;
                if (EnumExtension > 0) yield return ObjectType.EnumExtension;
            }
        }

        protected IEnumerable<ObjectIdAvailability> AvailableObjectIds =>
            ObjectIdAvailability
                .Invoke(Path, Recurse, RequestedObjectTypes)
                .Select(o => o.BaseObject)
                .Cast<ObjectIdAvailability>()
                .Where(a => a.Availability == Availability.Available);

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
            var sequence = AvailableObjectIds
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
            var ids = AvailableObjectIds.Where(c => c.ObjectType == objectType);

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