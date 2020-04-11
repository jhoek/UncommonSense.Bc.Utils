using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace UncommonSense.Bc.Utils
{
    [Cmdlet(VerbsCommon.Get, "BcObjectInfo")]
    [OutputType(typeof(ObjectInfo))]
    public class GetBcObjectInfoCmdlet : PSCmdlet
    {
        public static readonly Regex pattern = new Regex(@"^(?<ObjectType>[A-Za-z]+)\s+(?<ObjectID>\d+)\s+(?<ObjectName>.*?)(\s+extends\s+(?<BaseName>.*))?$");

        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty()]
        public string Path { get; set; } = ".";

        [Parameter()]
        public SwitchParameter Recurse { get; set; }

        // FIXME: Object Type Filtering
        // FIXME: Page customizations

        protected override void ProcessRecord() =>
            WriteObject(ObjectInfos, true);

        protected IEnumerable<ObjectInfo> ObjectInfos =>
            Matches.Select(m => new ObjectInfo(
                (ObjectType)Enum.Parse(typeof(ObjectType), m.Groups["ObjectType"].Value, true),
                int.Parse(m.Groups["ObjectID"].Value ?? "0"),
                m.Groups["ObjectName"].Value,
                m.Groups["BaseName"].Value
            ));

        protected IEnumerable<Match> Matches =>
            Signatures
                .Select(s => pattern.Match(s))
                .Where(m => m.Success);

        protected IEnumerable<string> Signatures =>
            FileNames
                .Select(f => File.ReadLines(f).FirstOrDefault())
                .Where(s => s != null);

        protected IEnumerable<string> FileNames =>
            Directory.Exists(NormalizedPath) ?
                Directory.GetFiles(NormalizedPath, "*.al", SearchOption) :
                new string[] { NormalizedPath };

        protected string NormalizedPath =>
            GetUnresolvedProviderPathFromPSPath(Path);

        protected SearchOption SearchOption =>
            Recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
    }
}