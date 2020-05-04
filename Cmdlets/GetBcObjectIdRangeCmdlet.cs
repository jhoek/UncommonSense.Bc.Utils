using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.Json;

namespace UncommonSense.Bc.Utils
{
    [Cmdlet(VerbsCommon.Get, "BcObjectIdRange")]
    [OutputType(typeof(ObjectIdRange))]
    public class GetBcObjectIdRangeCmdlet : PSCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty()]
        public string Path { get; set; } = ".";

        [Parameter()]
        [ValidateCount(1, int.MaxValue)]
        public ObjectType[] ObjectType { get; set; } = Helper.AllObjectTypes().ToArray();

        protected override void EndProcessing()
        {
            var path = GetUnresolvedProviderPathFromPSPath(Path);
            var appJsonPath = System.IO.Path.Combine(path, "app.json");

            if (!File.Exists(appJsonPath))
            {
                WriteWarning($"Cannot retrieve object ID ranges; no app.json file found in '{path}'.");
                return;
            }

            using (var document = JsonDocument.Parse(File.ReadAllText(appJsonPath)))
            {
                if (document.RootElement.TryGetProperty("idRanges", out var idRanges))
                    idRanges
                        .EnumerateArray()
                        .ForEach(r =>
                            ObjectType
                                .ForEach(t =>
                                    WriteObject(
                                        new ObjectIdRange(
                                            t,
                                            r.GetProperty("from").GetInt32(),
                                            r.GetProperty("to").GetInt32()
                                        )
                                    )
                                )
                        );
                else
                    WriteWarning($"Cannot retrieve object ID ranges; app.json file in {path} does not contain appropriate elements.");
            }
        }
    }
}