# Script de auditoria e traducao dos arquivos de locale
# Encoding: UTF-8 with BOM
# PowerShell 7+ required
param(
    [switch]$DryRun = $false
)

$ErrorActionPreference = "Stop"
$baseDir = "$PSScriptRoot\..\src\VianaHub.Global.Gerit.Api\locales"

# Universal terms that should NOT be translated
$universalTerms = @(
    "CSV", "UTF-8", "IBAN", "MIME", "JWT", "API", "URL", "HTTP", "JSON",
    "HH:mm", "dd/MM/yyyy",
    "Gerit v1", "API Health Check", "Healthy",
    "Stripe"
)

function IsUniversalTerm([string]$value) {
    foreach ($term in $universalTerms) {
        if ($value -eq $term) { return $true }
    }
    return $false
}

Write-Host "Loading files..."

# Read all files as raw JSON
$enUSRaw = Get-Content -Path "$baseDir\en-US\common.json" -Raw -Encoding UTF8
$ptPTRaw = Get-Content -Path "$baseDir\pt-PT\common.json" -Raw -Encoding UTF8
$ptBRRaw = Get-Content -Path "$baseDir\pt-BR\common.json" -Raw -Encoding UTF8
$esESRaw = Get-Content -Path "$baseDir\es-ES\common.json" -Raw -Encoding UTF8

# Parse JSON
$enUS = $enUSRaw | ConvertFrom-Json -AsHashtable
$ptPTHash = $ptPTRaw | ConvertFrom-Json -AsHashtable
$ptBRHash = $ptBRRaw | ConvertFrom-Json -AsHashtable
$esESHash = $esESRaw | ConvertFrom-Json -AsHashtable

Write-Host "Files loaded. en-US keys: $($enUS.Count)"

# Load translation dictionaries from JSON files
$dictPtPT = Get-Content -Path "$PSScriptRoot\dict-pt-PT.json" -Raw -Encoding UTF8 | ConvertFrom-Json -AsHashtable
$dictPtBR = Get-Content -Path "$PSScriptRoot\dict-pt-BR.json" -Raw -Encoding UTF8 | ConvertFrom-Json -AsHashtable
$dictEsES = Get-Content -Path "$PSScriptRoot\dict-es-ES.json" -Raw -Encoding UTF8 | ConvertFrom-Json -AsHashtable

Write-Host "Dictionaries loaded. pt-PT: $($dictPtPT.Count), pt-BR: $($dictPtBR.Count), es-ES: $($dictEsES.Count)"

# Generate translated files
$ptPTCount = 0
$ptBRCount = 0
$esESCount = 0

$ptPTOutput = [ordered]@{}
$ptBROutput = [ordered]@{}
$esESOutput = [ordered]@{}

foreach ($key in $enUS.Keys) {
    $enValue = $enUS[$key]
    
    # pt-PT
    $ptValue = if ($ptPTHash.ContainsKey($key)) { $ptPTHash[$key] } else { $null }
    if ($null -eq $ptValue) {
        $ptValue = ""
    }
    if (($ptValue -eq $enValue) -and (-not (IsUniversalTerm $enValue)) -and (-not [string]::IsNullOrWhiteSpace($enValue)) -and $dictPtPT.ContainsKey($enValue)) {
        $ptPTOutput[$key] = $dictPtPT[$enValue]
        $ptPTCount++
    } else {
        $ptPTOutput[$key] = $ptValue
    }

    # pt-BR
    $ptBRValue = if ($ptBRHash.ContainsKey($key)) { $ptBRHash[$key] } else { $null }
    if ($null -eq $ptBRValue) {
        $ptBRValue = ""
    }
    if (($ptBRValue -eq $enValue) -and (-not (IsUniversalTerm $enValue)) -and (-not [string]::IsNullOrWhiteSpace($enValue)) -and $dictPtBR.ContainsKey($enValue)) {
        $ptBROutput[$key] = $dictPtBR[$enValue]
        $ptBRCount++
    } else {
        $ptBROutput[$key] = $ptBRValue
    }

    # es-ES
    $esValue = if ($esESHash.ContainsKey($key)) { $esESHash[$key] } else { $null }
    if ($null -eq $esValue) {
        $esValue = ""
    }
    if (($esValue -eq $enValue) -and (-not (IsUniversalTerm $enValue)) -and (-not [string]::IsNullOrWhiteSpace($enValue)) -and $dictEsES.ContainsKey($enValue)) {
        $esESOutput[$key] = $dictEsES[$enValue]
        $esESCount++
    } else {
        $esESOutput[$key] = $esValue
    }
}

Write-Host "========================================"
Write-Host "Translation Results:"
Write-Host "  pt-PT: $ptPTCount values translated"
Write-Host "  pt-BR: $ptBRCount values translated"
Write-Host "  es-ES: $esESCount values translated"
Write-Host "  en-US keys: $($enUS.Count) (reference)"
Write-Host "  pt-PT keys: $($ptPTOutput.Count)"
Write-Host "  pt-BR keys: $($ptBROutput.Count)"
Write-Host "  es-ES keys: $($esESOutput.Count)"
Write-Host "========================================"

if ($DryRun) {
    Write-Host "DRY RUN - Files NOT written"
    exit 0
}

# Write files using System.Text.Json for proper Unicode handling
Add-Type -AssemblyName System.Text.Json

function Write-JsonFile {
    param($hash, $path)
    $options = New-Object System.Text.Json.JsonSerializerOptions
    $options.WriteIndented = $true
    $options.Encoder = [System.Text.Encodings.Web.JavaScriptEncoder]::UnsafeRelaxedJsonEscaping
    
    $json = [System.Text.Json.JsonSerializer]::Serialize($hash, $options)
    [System.IO.File]::WriteAllText($path, $json, [System.Text.UTF8Encoding]::new($false))
}

Write-JsonFile -hash $ptPTOutput -path "$baseDir\pt-PT\common.json"
Write-Host "Written: pt-PT\common.json"

Write-JsonFile -hash $ptBROutput -path "$baseDir\pt-BR\common.json"
Write-Host "Written: pt-BR\common.json"

Write-JsonFile -hash $esESOutput -path "$baseDir\es-ES\common.json"
Write-Host "Written: es-ES\common.json"

Write-Host "Done!"
