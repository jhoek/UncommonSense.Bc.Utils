Describe 'Get-BcObjectInfo' {
    It 'Reports the right objects' {
        $Results = Get-BcObjectInfo -Path $PSScriptRoot/../demo

        $Results | Should -HaveCount 7
    }

    It 'Reports the right objects (recursive)' {
        $Results = Get-BcObjectInfo -Path $PSScriptRoot/../demo -Recurse

        $Results | Should -HaveCount 9
    }

    It 'Filters based on object type' {
        $Results = Get-BcObjectInfo -Path $PSScriptRoot/../demo -Recurse -ObjectType Codeunit, Page

        $Results | Should -HaveCount 3
    }
}