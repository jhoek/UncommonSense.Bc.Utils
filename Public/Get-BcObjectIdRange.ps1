function Get-BcObjectIdRange
{
    param
    (
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty()]
        [string]$Path = '.'
    )

    $AppJsonPath = Join-Path -Path $Path -ChildPath 'app.json'

    if (-not (Test-Path -Path $AppJsonPath))
    {
        Write-Warning "Could not retrieve object id ranges; folder $Path does not contain a json.app file."
        return
    }

    Get-Content -Path $AppJsonPath `
    | ConvertFrom-Json -Depth 10 `
    | Select-Object -ExpandProperty idRanges `
    | ForEach-Object {
        $CurrentRange = $_

        [Enum]::GetValues([UncommonSense.Bc.Utils.ObjectType]) `
        | ForEach-Object {
            [UncommonSense.Bc.Utils.ObjectIdRange]::new(
                $_,
                $CurrentRange.From,
                $CurrentRange.To
            )
        }
}
}