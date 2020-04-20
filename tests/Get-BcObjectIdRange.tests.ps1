Describe 'Get-BcObjectIdRange' {
    It 'Returns the right ranges' {
        $Result = Get-BcObjectIdRange -Path $PSScriptRoot/../demo

        $Result | Should -HaveCount 12
        $Result | Where-Object FromObjectID -ne 50100 | Should -HaveCount 0
        $Result | Where-Object ToObjectID -ne 50105 | Should -HaveCount 0
        $Result | Select-Object -Property ObjectType -Unique | Should -HaveCount 12
    }
}