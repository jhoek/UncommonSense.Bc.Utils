function Get-BcObjectIdRange
{
    param
    (
        [Parameter(Mandatory, Position = 0)]
        [string]$Path    
    )

    $AppJsonPath = Join-Path -Path $Path -ChildPath 'app.json'

    if (-not (Test-Path -Path $AppJsonPath))
    {
        Write-Warning "Could not retrieve object id ranges; folder $Path does not contain a json.app file."
        return
    }

    Get-Content -Path $AppJsonPath `
    | ConvertFrom-Json -Depth 10 `
    | Select-Object -ExpandProperty idRanges        
}