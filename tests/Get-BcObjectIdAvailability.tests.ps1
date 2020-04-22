Get-Module UncommonSense.Bc.Utils -ListAvailable | Import-Module -Force

Add-AssertionOperator -Name ContainAvailability -Test {
    [CmdletBinding()]
    param
    (
        [Parameter(ValueFromPipeline)][UncommonSense.Bc.Utils.ObjectIdAvailability[]]$ActualValue,
        [UncommonSense.Bc.Utils.ObjectType]$ObjectType,
        [int]$ObjectID,
        [UncommonSense.Bc.Utils.Availability]$Availability,
        [switch]$Negate
    )

    begin
    {
        $CachedItems = New-Object -TypeName 'System.Collections.Generic.List[UncommonSense.Bc.Utils.ObjectIdAvailability]'
    }

    process
    {
        $CachedItems.AddRange($ActualValue)
    }

    end
    {
        [bool]$Succeeded = $ActualValue `
        | Where-Object ObjectType -eq $ObjectType 
        | Where-Object ObjectID -eq $ObjectID
        | Where-Object Availability -eq $Availability

    if ($Negate) { $Succeeded = -not $Succeeded }

    if (-not $Succeeded)
    {
        switch ($Negate)
        {
            $true { $FailureMessage = "Availability for $ObjectType $ObjectID should have not been $Availability" }
            $false { $FailureMessage = "Availability for $ObjectType $ObjectID should have been $Availability" }
        }
    }

    return New-Object -TypeName psobject -Property @{
        Succeeded      = $Succeeded
        FailureMessage = $FailureMessage
    }
}
}

Describe 'Get-BcObjectIdAvailability' {
    It 'Returns the correct availability statuses' {
        $Results = Get-BcObjectIdAvailability -Path $PSScriptRoot/../demo -Recurse 
        $Results | Should -HaveCount 72
        $Results | Should -ContainAvailability -ObjectType Table -ObjectID 50100 -Availability InUse    
    }
}
