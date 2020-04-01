function Get-BcObjectInfo 
{
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
            $CurrentFullName = $_.FullName
            $CurrentFileName = $_.Name

            Get-Content -Path $CurrentFullName -TotalCount 1 `
            | Select-String '^(?<Type>[A-Za-z]+)\s+(?<ID>\d+)\s+(?<Name>.*?)(\s+extends\s+(?<BaseName>.*))?$' `
            | ForEach-Object {
                [pscustomobject]@{
                    Path       = $CurrentFullName
                    FileName   = $CurrentFileName
                    Type       = $_.Matches[0].Groups['Type']
                    ID         = $_.Matches[0].Groups['ID']
                    Name       = $_.Matches[0].Groups['Name']                    
                    BaseName   = $_.Matches[0].Groups['BaseName']
                    PSTypeName = 'UncommonSense.Bc.Utils.ObjectInfo'
                }
            }
    }
}
}