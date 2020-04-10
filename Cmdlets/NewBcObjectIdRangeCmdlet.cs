using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "BcObjectIdRange", DefaultParameterSetName = ParameterSet.AllObjectTypes)]
    [OutputType(typeof(ObjectIdRange))]
    public class NewBcObjectIdRangeCmdlet : PSCmdlet
    {
        public static class ParameterSet
        {
            public const string SelectedObjectTypes = nameof(SelectedObjectTypes);
            public const string AllObjectTypes = nameof(AllObjectTypes);
        }

        [Parameter(Position = 0, ParameterSetName = ParameterSet.SelectedObjectTypes)]
        [ValidateCount(1, int.MaxValue)]
        public ObjectType[] ObjectType { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSet.AllObjectTypes)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSet.SelectedObjectTypes)]
        [ValidateRange(1, int.MaxValue)]
        public int FromObjectID { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSet.AllObjectTypes)]
        [Parameter(Mandatory = true, Position = 2, ParameterSetName = ParameterSet.SelectedObjectTypes)]
        [ValidateRange(1, int.MaxValue)]
        public int ToObjectID { get; set; }

        protected IEnumerable<ObjectType> EffectiveObjectTypes =>
            ParameterSetName == ParameterSet.SelectedObjectTypes ?
                ObjectType :
                ((ObjectType[])Enum.GetValues(typeof(ObjectType)));

        protected override void EndProcessing() =>
            WriteObject(EffectiveObjectTypes.Select(t => new ObjectIdRange(t, FromObjectID, ToObjectID)), true);
    }
}