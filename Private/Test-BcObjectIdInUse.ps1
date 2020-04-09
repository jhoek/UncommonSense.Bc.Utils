function Test-BcObjectIdInUse
{
    param
    (
        [Parameter(Mandatory, Position = 0)]
        [UncommonSense.Bc.Utils.ObjectType]$Type,

        [Parameter(Mandatory, Position = 1)]
        [ValidateRange(1, [int]::MaxValue)]
        [int]$ID,

        [Parameter(Mandatory)]
        [UncommonSense.Bc.Utils.ObjectIdInfo[]]$InUse
    )

    # Mag weg?

    $IdRangeResult = & $IdRange -Path $Path
    $InUseResult = & $InUse -Path $Path -Recurse:$Recurse

    [ValidateNotNull()]
    [UncommonSense.Bc.Utils.ObjectIdRange]$IdRange

    [ValidateNotNull()]
    [scriptblock]$InUse = { param([string]$Path, [switch]$Recurse) Get-BcObjectInfo -Path $Path -Recurse:$Recurse },
}