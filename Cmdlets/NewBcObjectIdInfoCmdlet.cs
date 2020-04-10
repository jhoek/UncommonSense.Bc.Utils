using System.Management.Automation;

namespace UncommonSense.Bc.Utils
{
    // Can be called e.g. by a script that collects reservations
    [Cmdlet(VerbsCommon.New, "BcObjectIdInfo")]
    public class NewBcObjectIdInfoCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public ObjectType ObjectType { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public int ObjectID { get; set; }

        protected override void EndProcessing()
        {
            WriteObject(new ObjectIdInfo(ObjectType, ObjectID));
        }
    }
}