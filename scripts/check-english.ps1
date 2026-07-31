param($lang = "pt-PT")

$ErrorActionPreference = "Stop"
$baseDir = "src/VianaHub.Global.Gerit.Api/locales"

$enUS = Get-Content "$baseDir/en-US/common.json" -Raw -Encoding UTF8 | ConvertFrom-Json -AsHashtable
$target = Get-Content "$baseDir/$lang/common.json" -Raw -Encoding UTF8 | ConvertFrom-Json -AsHashtable

$universal = @("CSV", "UTF-8", "IBAN", "MIME", "JWT", "API", "URL", "HTTP", "JSON", "HH:mm", "dd/MM/yyyy", "Stripe", "Gerit v1", "API Health Check", "Healthy")
$count = 0

foreach ($key in $enUS.Keys) {
    $enVal = $enUS[$key]
    if ($target.ContainsKey($key)) {
        $tVal = $target[$key]
        if ($tVal -eq $enVal -and $enVal -notin $universal -and -not [string]::IsNullOrWhiteSpace($enVal)) {
            Write-Host "$key = $enVal"
            $count++
        }
    }
}

Write-Host "Total remaining en-US values in $lang : $count"
