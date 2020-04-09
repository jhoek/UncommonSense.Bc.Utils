function New-BcObjectIdRange
{
    param
    (
        [Parameter(Mandatory, Position = 0)]
        [UncommonSense.Bc.Utils.ObjectType[]]$Type,

        [Parameter(Mandatory, Position = 1)]
        [ValidateRange(1, [int]::MaxValue)]
        [int]$FromID,

        [Parameter(Mandatory, Position=2)]
        [ValidateRange(1, [int]::MaxValue)]
        [int]$ToID
    )

    $Type.ForEach{ [UncommonSense.Bc.Utils.ObjectIdRange]::new($_, $FromID, $ToID) }
}