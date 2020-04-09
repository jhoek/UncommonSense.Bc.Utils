function Get-BcObjectInfo
{
    [OutputType([UncommonSense.Bc.Utils.ObjectIdInfo])]
    param
    (
        [Parameter(ValueFromPipeline, ValueFromPipelineByPropertyName, Position = 0)]
        [ValidateNotNullOrEmpty()]
        [string[]]$Path = '.',

        [switch]$Recurse
    )

    process
    {
        Get-ChildItem -Path $Path -Filter *.al -File -Recurse:$Recurse `
        | ForEach-Object {
            Get-Content -Path $_ -TotalCount 1 `
            | Select-String '^(?<Type>[A-Za-z]+)\s+(?<ID>\d+)\s+(?<Name>.*?)(\s+extends\s+(?<BaseName>.*))?$' `
            | ForEach-Object {
                [UncommonSense.Bc.Utils.ObjectInfo]::new(
                    $_.Matches[0].Groups['Type'].Value,
                    $_.Matches[0].Groups['ID'].Value,
                    $_.Matches[0].Groups['Name'].Value,
                    $_.Matches[0].Groups['BaseName'].Value
                )
            }
    } | Sort-Object Type, ID
}
}