using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    [Cmdlet(VerbsCommon.Find, "AvailableBcObjectId")]
    [OutputType(typeof(ObjectIdInfo))]
    public class FindAvailableBcObjectIdCmdlet : Cmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty()]
        public string Path { get; set; } = ".";

        [Parameter()]
        [ValidateNotNull()]
        public ScriptBlock GetObjectIdAvailabity { get; set; } = ScriptBlock.Create("param([string]$Path) Get-BcObjectIdAvailability -Path $Path");

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int Tables { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int Pages { get; set; }

        // use lowest id first, contiguous range per object type, same ids for all object types        

    }
}