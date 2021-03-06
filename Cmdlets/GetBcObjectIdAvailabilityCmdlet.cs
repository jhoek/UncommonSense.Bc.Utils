using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    [Cmdlet(VerbsCommon.Get, "BcObjectIdAvailability", DefaultParameterSetName = ParameterSet.Details)]
    [OutputType(typeof(Availability), ParameterSetName = new string[] { ParameterSet.Details })]
    [OutputType(typeof(ObjectIdAvailabilitySummaryItem), ParameterSetName = new string[] { ParameterSet.Summary })]
    public class GetBcObjectIdAvailabilityCmdlet : PSCmdlet
    {
        public static class ParameterSet
        {
            public const string Details = nameof(Details);
            public const string Summary = nameof(Summary);
        }

        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty()]
        public string Path { get; set; } = ".";

        [Parameter()]
        public SwitchParameter Recurse { get; set; }

        [Parameter()]
        [ValidateNotNull()]
        public ObjectType[] ObjectType { get; set; } = Helper.AllObjectTypes().ToArray();

        [Parameter()]
        [ValidateNotNull()]
        public ScriptBlock IdRange { get; set; } = ScriptBlock.Create("param([string]$Path, [UncommonSense.Bc.Utils.ObjectType[]]$ObjectType) Get-BcObjectIdRange -Path $Path -ObjectType $ObjectType");

        [Parameter()]
        [ValidateNotNull()]
        public ScriptBlock Reserved { get; set; } = ScriptBlock.Create("param([string]$Path, [UncommonSense.Bc.Utils.ObjectType[]]$ObjectType) @()");

        [Parameter()]
        [ValidateNotNull()]
        public ScriptBlock InUse { get; set; } = ScriptBlock.Create("param([string]$Path, [UncommonSense.Bc.Utils.ObjectType[]]$ObjectType, [switch]$Recurse) Get-BcObjectInfo -Path $Path -ObjectType $ObjectType -Recurse:$Recurse");

        [Parameter(ParameterSetName = ParameterSet.Summary)]
        public SwitchParameter Summary { get; set; }

        protected override void EndProcessing()
        {
            var idRanges = IdRange.Invoke(Path, ObjectType).Select(o => o.BaseObject).Cast<ObjectIdRange>();
            var reserved = Reserved.Invoke(Path, ObjectType).Select(o => o.BaseObject).Cast<ObjectIdInfo>();
            var inUse = InUse.Invoke(Path, ObjectType, Recurse).Select(o => o.BaseObject).Cast<ObjectIdInfo>();

            if (MyInvocation.BoundParameters.ContainsKey(nameof(ObjectType)))
            {
                idRanges = idRanges.Where(r => ObjectType.Contains(r.ObjectType));
                reserved = reserved.Where(r => ObjectType.Contains(r.ObjectType));
                inUse = inUse.Where(o => ObjectType.Contains(o.ObjectType));
            }

            switch (Summary.IsPresent)
            {
                case true: WriteSummary(idRanges, reserved, inUse); break;
                case false: WriteDetails(idRanges, reserved, inUse); break;
            }
        }

        protected void WriteSummary(IEnumerable<ObjectIdRange> idRanges, IEnumerable<ObjectIdInfo> reserved, IEnumerable<ObjectIdInfo> inUse)
        {
            var result = new ObjectIdAvailabilitySummary(ObjectType);
            idRanges.ForEach(r => { if (!result.AddIdRange(r)) WriteWarning("The ID ranges specified appear to overlap."); });
            reserved.ForEach(i => { if (!result.AddReservedObject(i)) WriteWarning($"Reservation for {i.ObjectType} {i.ObjectID} lies outside of the available ID ranges."); });
            inUse.ForEach(i => { if (!result.AddUsedObject(i)) WriteWarning($"ID for {i.ObjectType} {i.ObjectID} lies outside of the available ID ranges."); });
            WriteObject(result);
        }

        protected void WriteDetails(IEnumerable<ObjectIdRange> idRanges, IEnumerable<ObjectIdInfo> reserved, IEnumerable<ObjectIdInfo> inUse)
        {
            var progressRecord = new ProgressRecord(1, "Calculating BC object ID availability", "Initializing");
            WriteProgress(progressRecord);

            WriteObject(
                idRanges.SelectMany(r =>
                    r.IDs.Select(i => new ObjectIdAvailability(r.ObjectType, i, CalculateAvailability(r.ObjectType, i, reserved, inUse, progressRecord)))
                ),
                true
            );
        }

        protected Availability CalculateAvailability(ObjectType objectType, int objectID, IEnumerable<ObjectIdInfo> reserved, IEnumerable<ObjectIdInfo> inUse, ProgressRecord progressRecord)
        {
            progressRecord.StatusDescription = $"{objectType} {objectID}";
            WriteProgress(progressRecord);

            return inUse.Any(o => o.ObjectType == objectType && o.ObjectID == objectID) ?
                Availability.InUse :
                reserved.Any(o => o.ObjectType == objectType && o.ObjectID == objectID) ?
                    Availability.Reserved :
                    Availability.Available;
        }
    }
}