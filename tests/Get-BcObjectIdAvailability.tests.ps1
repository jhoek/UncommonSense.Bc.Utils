Describe 'Get-BcObjectIdAvailability' {
    It 'Returns the correct availability statuses' {
        $Results = Get-BcObjectIdAvailability -Path $PSScriptRoot/../demo -Recurse 
        $Results | Should -HaveCount 72

        function Test-BcObjectIdAvailability
        {
            param
            (
                [Parameter(Mandatory)]
                [UncommonSense.Bc.Utils.ObjectIdAvailability[]]$ActualValue,

                [Parameter(Mandatory)]
                [UncommonSense.Bc.Utils.ObjectType]$ObjectType,

                [Parameter(Mandatory)]
                [int[]]$ObjectID,

                [Parameter(Mandatory)]
                [UncommonSense.Bc.Utils.Availability]$Availability
            )

            $ObjectID.ForEach{
                $Found = [bool](
                    $ActualValue `
                    | Where-Object ObjectType -eq $ObjectType `
                    | Where-Object ObjectID -eq $_ `
                    | Where-Object Availability -eq $Availability
            )
                    
            if (-not $Found)
            {
                Write-Error -Message "Availability for $ObjectType $_ should have been $Availability"
            }
        }            
    }

    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType Table -ObjectID 50102 -Availability InUse    
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType Table -ObjectID (50100..50101) -Availability Available
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType Table -ObjectID (50103..50105) -Availability Available
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType TableExtension -ObjectID 50100 -Availability InUse    
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType TableExtension -ObjectID (50101..50105) -Availability Available
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType Page -ObjectID 50101 -Availability InUse
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType Page -ObjectID 50100 -Availability Available
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType Page -ObjectID (50102..50105) -Availability Available
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType PageExtension 50100 -Availability InUse
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType PageExtension (50101..50105) -Availability Available
    Test-BcObjectIdAvailability -ActualValue $Results -ObjectType Report (50100..50105) -Availability Available
}
}
