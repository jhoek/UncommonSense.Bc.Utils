using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    // Can be called e.g. by a script that collects reservations    
    [Cmdlet(VerbsCommon.New, "ObjectIdInfo")]
    public class NewObjectIdInfoCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public ObjectType Type { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public int ID { get; set; }

        protected override void EndProcessing()
        {
            WriteObject(new ObjectIdInfo(Type, ID));
        }
    }
}