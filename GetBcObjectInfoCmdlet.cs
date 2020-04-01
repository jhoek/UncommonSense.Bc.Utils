using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    [Cmdlet(VerbsCommon.Get, "BcObjectInfo")]
    [OutputType(typeof(ObjectInfo))]
    public class GetBcObjectInfoCmdlet : PSCmdlet
    {
        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [ValidateNotNullOrEmpty()]
        public string[] Path { get; set; } = new string[] { "." };

        [Parameter()]
        public SwitchParameter Recurse { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(
                FilePaths
                    .Select(f => File.ReadLines(f).FirstOrDefault())
                    .SelectMany(l => ObjectInfo.Create(l)),
                true
            );
        }

        protected IEnumerable<string> FilePaths =>
            ResolvedPaths.SelectMany(p => Directory.Exists(p) ? Directory.GetFiles(p, "*.al", SearchOption) : new string[] { p });

        protected IEnumerable<string> ResolvedPaths =>
            Path.SelectMany(p => GetResolvedProviderPathFromPSPath(p, out ProviderInfo provider));

        protected SearchOption SearchOption =>
            Recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
    }
}