using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    [Cmdlet(VerbsCommon.Get, "BcObjectIdAvailability", DefaultParameterSetName = ParameterSet.Details)]
    [OutputType(typeof(Availability), ParameterSetName = new string[] { ParameterSet.Details })]
    [OutputType(typeof(ObjectIdAvailabilitySummary), ParameterSetName = new string[] { ParameterSet.Summary })]
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
        public ScriptBlock IdRange { get; set; } = ScriptBlock.Create("param([string]$Path) Get-BcObjectIdRange -Path $Path");

        [Parameter()]
        [ValidateNotNull()]
        public ScriptBlock Reserved { get; set; } = ScriptBlock.Create("@()");

        [Parameter()]
        [ValidateNotNull()]
        public ScriptBlock InUse { get; set; } = ScriptBlock.Create("param([string]$Path, [switch]$Recurse) Get-BcObjectInfo -Path $Path -Recurse:$Recurse");

        [Parameter()]
        public ObjectType[] ObjectType { get; set; }

        [Parameter(ParameterSetName = ParameterSet.Summary)]
        public SwitchParameter Summary { get; set; }

        protected override void EndProcessing()
        {
            var idRanges = IdRange.Invoke(Path).Select(o => o.BaseObject).Cast<ObjectIdRange>();
            var reserved = Reserved.Invoke(Path).Select(o => o.BaseObject).Cast<ObjectIdInfo>();
            var inUse = InUse.Invoke(Path, Recurse).Select(o => o.BaseObject).Cast<ObjectIdInfo>();

            if (MyInvocation.BoundParameters.ContainsKey(nameof(ObjectType)))
            {
                idRanges = idRanges.Where(r => ObjectType.Contains(r.ObjectType));
                reserved = reserved.Where(r => ObjectType.Contains(r.ObjectType));
                inUse = inUse.Where(o => ObjectType.Contains(o.ObjectType));
            }

            switch (Summary.IsPresent)
            {
                case true: WriteSummary(idRanges); break;
                case false: WriteDetails(idRanges, reserved, inUse); break;
            }
        }

        protected void WriteSummary(IEnumerable<ObjectIdRange> idRanges)
        {
            var result = new List<ObjectIdAvailabilitySummary>();




            WriteObject(result, true);
        }

        protected void WriteDetails(IEnumerable<ObjectIdRange> idRanges, IEnumerable<ObjectIdInfo> reserved, IEnumerable<ObjectIdInfo> inUse)
        {
            WriteObject(
                idRanges.SelectMany(r =>
                    Enumerable
                        .Range(r.FromObjectID, r.ToObjectID - r.FromObjectID + 1)
                        .Select(i => new ObjectIdAvailability(r.ObjectType, i, CalculateAvailability(r.ObjectType, i, reserved, inUse)))
                ),
                true
            );
        }

        protected Availability CalculateAvailability(ObjectType objectType, int objectID, IEnumerable<ObjectIdInfo> reserved, IEnumerable<ObjectIdInfo> inUse) =>
            inUse.Any(o => o.ObjectType == objectType && o.ObjectID == objectID) ?
                Availability.InUse :
                reserved.Any(o => o.ObjectType == objectType && o.ObjectID == objectID) ?
                    Availability.Reserved :
                    Availability.Available;
    }
}