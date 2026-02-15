$root = "src\VianaHub.Global.Gerit.Api\Localization"
$bases = @('swagger','api','application','domain')
$cultures = @('en-US','pt-PT','es-ES')

foreach ($b in $bases) {
    $enPath = Join-Path $root "$b.en-US.json"
    if (-not (Test-Path $enPath)) { Write-Host "Missing master: $enPath"; continue }

    $en = Get-Content $enPath -Raw | ConvertFrom-Json
    $enKeys = $en.psobject.properties.name

    foreach ($cult in $cultures) {
        if ($cult -eq 'en-US') { continue }
        $targetPath = Join-Path $root "$b.$cult.json"
        $targetExists = Test-Path $targetPath
        $target = $null
        if ($targetExists) { $target = Get-Content $targetPath -Raw | ConvertFrom-Json }

        $ordered = [ordered]@{}
        foreach ($k in $enKeys) {
            $val = $null
            if ($target -ne $null -and $target.psobject.properties.name -contains $k) {
                $val = $target.$k
            } else {
                $val = $en.$k
            }
            $ordered[$k] = $val
        }

        $json = $ordered | ConvertTo-Json -Depth 10
        # Pretty-print with indentation of 2 spaces
        $json = $json -replace '(^\s*)"', '$1"' # no-op to keep format
        [System.IO.File]::WriteAllText($targetPath, $json)
        Write-Host "Wrote $targetPath"
    }
}
Write-Host 'Done.'
