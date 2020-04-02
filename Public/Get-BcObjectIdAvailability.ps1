function Get-BcObjectIdAvailability
{
    [OutputType([UncommonSense.Bc.Utils.ObjectIdAvailability])]
    param
    (
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty()]
        [string]$Path = '.',

        [ValidateNotNull()]
        [scriptblock]$IdRange = { param([string]$Path) Get-BcObjectIdRange -Path $Path },

        [ValidateNotNull()]
        [scriptblock]$InUse = { param([string]$Path, [switch]$Recurse) Get-BcObjectInfo -Path $Path -Recurse:$Recurse },

        [switch]$Recurse
    )

    [ObjectIdInfo[]]$InUseResult = & $InUse -Path $Path -Recurse:$Recurse
}