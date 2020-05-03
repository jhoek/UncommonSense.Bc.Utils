Describe 'Find-AvailableBcObjectId' {
    function Test-BcObjectIdInfo
    {
        param(
            [Parameter(Mandatory)]
            [UncommonSense.Bc.Utils.ObjectIdInfo[]]$ActualValue,

            [Parameter(Mandatory)]
            [UncommonSense.Bc.Utils.ObjectType]$ObjectType,

            [Parameter(Mandatory)]
            [int[]]$ObjectID
        )

        $ObjectID.ForEach{
            $Found = [bool](
                $ActualValue `
                | Where-Object ObjectType -eq $ObjectType `
                | Where-Object ObjectID -eq $_ 
        )

        if (-not $Found)
        {
            Write-Error -Message "$ObjectType $_ should have been present in the results"                
        }
    }
}

It 'Returns a simple range' {
    $Results = Find-AvailableBcObjectId -Path $PSScriptRoot/../demo -Report 4
    $Results | Should -HaveCount 4
    
    Test-BcObjectIdInfo -ActualValue $Results -ObjectType Report -ObjectID (50100..50103)
}

It 'Reports when a simple range cannot be found' {
    { Find-AvailableBcObjectId -Path $PSScriptRoot/../demo -Report 7 -ErrorAction Stop } `
    | Should -Throw -ExpectedMessage 'Could not find 7 available Report IDs.'
}

It 'Returns a broken range' {
    $Results = Find-AvailableBcObjectId -Path $PSScriptRoot/../demo -Table 3
    $Results | Should -HaveCount 3

    Test-BcObjectIdInfo -ActualValue $Results -ObjectType Table -ObjectID (50100..50101)
    Test-BcObjectIdInfo -ActualValue $Results -ObjectType Table -ObjectID (50103)    
}

It 'Returns a contiguous range' {
    $Results = Find-AvailableBcObjectId -Path $PSScriptRoot/../demo -Table 3 -Contiguous
    $Results | Should -HaveCount 3

    Test-BcObjectIdInfo -ActualValue $Results -ObjectType Table -ObjectID (50103..50105)
}

It 'Reports when a contiguous range cannot be found' {
    { Find-AvailableBcObjectId -Path $PSScriptRoot/../demo -Table 4 -Contiguous -ErrorAction Stop } `
    | Should -Throw -ExpectedMessage 'Could not find a contiguous range of 4 available Table IDs.'
}
}