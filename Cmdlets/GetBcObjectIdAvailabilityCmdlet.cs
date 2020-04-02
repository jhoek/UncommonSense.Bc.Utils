using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    [Cmdlet(VerbsCommon.Get, "BcObjectIdAvailability", DefaultParameterSetName = ParameterSet.Details)]
    [OutputType(typeof(Availability), ParameterSetName = new string[] { ParameterSet.Details })]
    [OutputType(typeof(ObjectIdAvailabilitySummary), ParameterSetName = new string[] { ParameterSet.Summary })]
    public class GetBcObjectStatusCmdlet : Cmdlet
    {
        public static class ParameterSet
        {
            public const string Details = nameof(Details);
            public const string Summary = nameof(Summary);
        }

        [Parameter()]
        [ValidateNotNull()]
        public ScriptBlock InUse { get; set; } = ScriptBlock.Create("Get");



        // In: Reserved
        // In: ranges

        [Parameter(ParameterSetName = ParameterSet.Summary)]
        public SwitchParameter Summary { get; set; }
    }
}