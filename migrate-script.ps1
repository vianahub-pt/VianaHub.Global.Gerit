# Script de migracao de TeamMember para Employee
$ErrorActionPreference = "Stop"
$basePath = "C:\git\my\VianaHub.Global.Gerit\src"

Write-Host "=== Iniciando migracao TeamMember -> Employee ===" -ForegroundColor Green

# Funcao para renomear arquivo
function Rename-File {
    param($filePath)
    
    $newPath = $filePath
    $newPath = $newPath -replace 'TeamMembersTeam', 'EmployeeTeam'
    $newPath = $newPath -replace 'TeamMembers', 'Employees'  
    $newPath = $newPath -replace 'TeamMember', 'Employee'
    
    if ($newPath -ne $filePath -and (Test-Path $filePath)) {
        $newDir = Split-Path -Parent $newPath
        if (!(Test-Path $newDir)) {
            New-Item -ItemType Directory -Path $newDir -Force | Out-Null
        }
        
        Move-Item -Path $filePath -Destination $newPath -Force
        Write-Host "Renomeado: $(Split-Path -Leaf $filePath) -> $(Split-Path -Leaf $newPath)" -ForegroundColor Cyan
        return $newPath
    }
    return $filePath
}

# Funcao para atualizar conteudo do arquivo
function Update-FileContent {
    param($filePath)

    if (!(Test-Path $filePath)) {
        return
    }

    try {
        $content = Get-Content -Path $filePath -Raw -Encoding UTF8
        $originalContent = $content

        $content = $content -replace 'TeamMembersTeam', 'EmployeeTeam'
        $content = $content -replace 'teamMembersTeam', 'employeeTeam'
        $content = $content -replace 'TeamMembers', 'Employees'
        $content = $content -replace 'teamMembers', 'employees'
        $content = $content -replace 'TeamMember', 'Employee'
        $content = $content -replace 'teamMember', 'employee'

        if ($content -ne $originalContent) {
            Set-Content -Path $filePath -Value $content -Encoding UTF8 -NoNewline
            Write-Host "  Conteudo atualizado: $(Split-Path -Leaf $filePath)" -ForegroundColor Yellow
        }
    }
    catch {
        Write-Host "  AVISO: Arquivo bloqueado: $(Split-Path -Leaf $filePath)" -ForegroundColor Red
    }
}

# 1. Encontrar todos os arquivos TeamMember
Write-Host "`n1. Procurando arquivos TeamMember..." -ForegroundColor Green
$files = Get-ChildItem -Path $basePath -Recurse -Filter "*TeamMember*" -File | 
    Where-Object { 
        $_.Extension -eq '.cs' -and 
        $_.Name -ne 'EmployeeEntity.cs' -and 
        $_.Name -ne 'EmployeeContactEntity.cs' -and 
        $_.Name -ne 'EmployeeAddressEntity.cs' -and
        $_.Name -notlike '*.old'
    }

Write-Host "Encontrados $($files.Count) arquivos para processar" -ForegroundColor White

# 2. Renomear arquivos
Write-Host "`n2. Renomeando arquivos..." -ForegroundColor Green
$renamedFiles = @()
foreach($file in $files) {
    $newPath = Rename-File -filePath $file.FullName
    $renamedFiles += $newPath
}

# 3. Atualizar conteudo de TODOS os arquivos .cs
Write-Host "`n3. Atualizando conteudo dos arquivos..." -ForegroundColor Green
$allCsFiles = Get-ChildItem -Path $basePath -Recurse -Filter "*.cs" -File | 
    Where-Object { $_.Name -notlike '*.old' }

Write-Host "Atualizando $($allCsFiles.Count) arquivos .cs" -ForegroundColor White
foreach($file in $allCsFiles) {
    Update-FileContent -filePath $file.FullName
}

# 4. Renomear diretorios
Write-Host "`n4. Renomeando diretorios..." -ForegroundColor Green
$directories = Get-ChildItem -Path $basePath -Recurse -Directory | 
    Where-Object { $_.Name -like "*TeamMember*" } | 
    Sort-Object { $_.FullName.Length } -Descending

foreach($dir in $directories) {
    $newDirPath = $dir.FullName
    $newDirPath = $newDirPath -replace 'TeamMembersTeam', 'EmployeeTeam'
    $newDirPath = $newDirPath -replace 'TeamMembers', 'Employees'
    $newDirPath = $newDirPath -replace 'TeamMember', 'Employee'
    
    if ($newDirPath -ne $dir.FullName) {
        $parentDir = Split-Path -Parent $newDirPath
        if (!(Test-Path $parentDir)) {
            New-Item -ItemType Directory -Path $parentDir -Force | Out-Null
        }
        Move-Item -Path $dir.FullName -Destination $newDirPath -Force
        Write-Host "Diretorio renomeado: $($dir.Name) -> $(Split-Path -Leaf $newDirPath)" -ForegroundColor Cyan
    }
}

Write-Host "`n=== Migracao concluida ===" -ForegroundColor Green
Write-Host "Arquivos processados: $($files.Count)" -ForegroundColor White
Write-Host "Arquivos atualizados: $($allCsFiles.Count)" -ForegroundColor White
