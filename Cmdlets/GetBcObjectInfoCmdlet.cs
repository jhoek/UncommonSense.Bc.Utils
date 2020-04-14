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
        public static readonly Regex normalObjects = new Regex(@"^(?<ObjectType>[A-Za-z]+)\s+(?<ObjectID>\d+)\s+(?<ObjectName>.*?)(\s+extends\s+(?<BaseName>.*))?$", RegexOptions.IgnoreCase);
        public static readonly Regex pageCustomizations = new Regex(@"^(?<ObjectType>PageCustomization)\s+(?<ObjectName>.*?)\s+customizes\s+(?<BaseName>.*)$", RegexOptions.IgnoreCase);

        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty()]
        public string Path { get; set; } = ".";

        [Parameter()]
        [ValidateNotNull()]
        public ObjectType[] ObjectType { get; set; } = Helper.AllObjectTypes().ToArray();

        [Parameter()]
        public SwitchParameter Recurse { get; set; }

        protected override void ProcessRecord() =>
            WriteObject(
                ObjectInfos
                    .Where(o => ObjectType.Contains(o.ObjectType)),
                true
            );

        protected IEnumerable<ObjectInfo> ObjectInfos =>
            Matches
                .ForEach(m => WriteVerbose($"    Match {m}"))
                .Select(m => new ObjectInfo(
                    (ObjectType)Enum.Parse(typeof(ObjectType), m.Groups["ObjectType"].Value, true),
                    int.Parse(m.Groups["ObjectID"].Success ? m.Groups["ObjectID"].Value : "0"),
                    m.Groups["ObjectName"].Value,
                    m.Groups["BaseName"].Value
                ));

        protected IEnumerable<Match> Matches =>
            Signatures
                .ForEach(s => WriteVerbose($"  Signature {s}"))
                .Select(s => new { input = s, match = normalObjects.Match(s) })
                .Select(o => o.match.Success ? o.match : pageCustomizations.Match(o.input))
                .Where(m => m.Success);

        protected IEnumerable<string> Signatures =>
            FileNames
                .ForEach(f => WriteVerbose($"File {f}"))
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