# Script to add missing translation entries to dictionaries
param($lang = "pt-PT")

$ErrorActionPreference = "Stop"
$baseDir = "src/VianaHub.Global.Gerit.Api/locales"
$scriptDir = "scripts"

# Load en-US and target
$enUS = Get-Content "$baseDir/en-US/common.json" -Raw -Encoding UTF8 | ConvertFrom-Json -AsHashtable
$target = Get-Content "$baseDir/$lang/common.json" -Raw -Encoding UTF8 | ConvertFrom-Json -AsHashtable
$dict = Get-Content "$scriptDir/dict-$lang.json" -Raw -Encoding UTF8 | ConvertFrom-Json -AsHashtable

$universal = @("CSV", "UTF-8", "IBAN", "MIME", "JWT", "API", "URL", "HTTP", "JSON", "HH:mm", "dd/MM/yyyy", "Stripe", "Gerit v1", "API Health Check", "Healthy")

# Find values that are still in English
$missing = @{}
foreach ($key in $enUS.Keys) {
    $enVal = $enUS[$key]
    if ($target.ContainsKey($key)) {
        $tVal = $target[$key]
        if ($tVal -eq $enVal -and $enVal -notin $universal -and -not [string]::IsNullOrWhiteSpace($enVal) -and -not $dict.ContainsKey($enVal)) {
            if (-not $missing.ContainsKey($enVal)) {
                $missing[$enVal] = @()
            }
            $missing[$enVal] += $key
        }
    }
}

Write-Host "Missing dictionary entries for $lang : $($missing.Count)"
foreach ($val in $missing.Keys | Sort-Object) {
    Write-Host "  `"$val`""
}
