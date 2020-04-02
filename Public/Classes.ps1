class ObjectIdInfo 
{
    ObjectIdInfo([UncommonSense.BC.Utils.ObjectType]$Type, [int]$ID)
    {
        $this.Type = $Type
        $this.ID = $ID
    }

    [ValidateNotNullOrEmpty()][UncommonSense.BC.Utils.ObjectType]$Type
    [ValidateRange(1, [int]::MaxValue)][int]$ID
}

class ObjectInfo : ObjectIdInfo
{
    ObjectInfo([UncommonSense.BC.Utils.ObjectType]$Type, [int]$ID, [string]$Name, [string]$BaseName) : base($Type, $ID)
    {
        $this.Name = $Name
        $this.BaseName = $BaseName
    }

    [ValidateNotNullOrEmpty()][string]$Name
    [string]$BaseName
}