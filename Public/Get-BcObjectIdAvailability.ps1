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

        [switch]$Summary,

        [switch]$Recurse
    )

    [UncommonSense.Bc.Utils.ObjectIdRange[]]$IdRangeResult = & $IdRange -Path $Path
    [UncommonSense.Bc.Utils.ObjectIdInfo[]]$InUseResult = & $InUse -Path $Path -Recurse:$Recurse

    if ($Summary)
    {
        $AllIds = @{ }

        $IdRangeResult `
        | ForEach-Object {
            $CurrentRange = $_

            ($CurrentRange.From)..($CurrentRange.To) `
            | ForEach-Object {
                $CurrentID = $_

                if (-not $AllIds.ContainsKey($CurrentID))
                {
                    $AllIds[$CurrentID] = @{
                        [UncommonSense.Bc.Utils.ObjectType].GetEnumValues()
                    }

                }
            }

        $AllIds
    }
}
else
{
    $IdRangeResult `
    | ForEach-Object {
        $CurrentRange = $_

        ($CurrentRange.From)..($CurrentRange.To) `
        | ForEach-Object {
            [UncommonSense.Bc.Utils.ObjectIdAvailability]::new(
                $CurrentRange.Type,
                $_,
                [UncommonSense.Bc.Utils.Availability]::InUse
            )
        }
}
}
}