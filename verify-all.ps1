# verify-all.ps1 - kontrollib kogu KooliProjekt lahendust otsast lõpuni
# Kasutamine:  powershell -ExecutionPolicy Bypass -File .\verify-all.ps1
# NB! Kasuta projektifailide teedel alati kaldkriipsu (/), mitte tagurpidi kaldkriipsu.

$ErrorActionPreference = "Continue"
Set-Location $PSScriptRoot

$results = @()
function Add-Result($nimi, $ok, $detail) {
    $script:results += [pscustomobject]@{
        Kontroll = $nimi
        Tulemus  = $(if ($ok) { "OK" } else { "FAIL" })
        Detail   = $detail
    }
}

# --- 1. Lahenduse build -----------------------------------------------------
Write-Host "`n=== 1/6  Build (KooliProjekt.sln) ===" -ForegroundColor Cyan
$out = dotnet build KooliProjekt.sln -v q --nologo 2>&1
$ok = ($LASTEXITCODE -eq 0)
$detail = ($out | Select-String -Pattern "Error\(s\)" | Select-Object -First 1)
Add-Result "Build (kõik projektid)" $ok "$detail"
if (-not $ok) { $out | Select-Object -Last 25 }

# --- abifunktsioon: üks testiprojekt ---------------------------------------
function Test-Project($nimi, $csproj) {
    Write-Host "`n=== dotnet test: $nimi ===" -ForegroundColor Cyan
    $o = dotnet test $csproj --nologo -v q 2>&1
    $ok = ($LASTEXITCODE -eq 0)
    $line = ($o | Select-String -Pattern "Passed!|Failed!|Passed:|error" | Select-Object -Last 1)
    Add-Result $nimi $ok "$line"
    if (-not $ok) { $o | Select-Object -Last 25 }
}

# --- 2. Rakenduse ühiktestid (16.01, 22.01, 23.01, 05.02, 06.02) -----------
Test-Project "Application.UnitTests (150)" "KooliProjekt.Application.UnitTests/KooliProjekt.Application.UnitTests.csproj"

# --- 3. Integratsioonitestid (13.02, 19.02) ---------------------------------
Test-Project "IntegrationTests (44)" "KooliProjekt.IntegrationTests/KooliProjekt.IntegrationTests.csproj"

# --- 4. Windows Formsi presenteri testid (02.04) ----------------------------
Test-Project "WindowsForms.UnitTests (9)" "KooliProjekt.WindowsForms.UnitTests/KooliProjekt.WindowsForms.UnitTests.csproj"

# --- 5. WPF view modeli testid (17.04) --------------------------------------
Test-Project "WpfApplication.UnitTests (12)" "KooliProjekt.WpfApplication.UnitTests/KooliProjekt.WpfApplication.UnitTests.csproj"

# --- 6. Testide raport + coverage (12.02) ----------------------------------
Write-Host "`n=== 6/6  Testide raport (run-tests.ps1) ===" -ForegroundColor Cyan
$raport = "KooliProjekt.Application.UnitTests/run-tests.ps1"
if (Test-Path $raport) {
    $o = & powershell -NoProfile -ExecutionPolicy Bypass -File $raport 2>&1
    $ok = ($LASTEXITCODE -eq 0)
    Add-Result "Coverage raport" $ok "vaata KooliProjekt.Application.UnitTests/BuildReports"
    if (-not $ok) { $o | Select-Object -Last 20 }
} else {
    Add-Result "Coverage raport" $false "run-tests.ps1 ei leitud"
}

# --- kokkuvõte --------------------------------------------------------------
Write-Host "`n================ KOKKUVOTE ================" -ForegroundColor Yellow
$results | Format-Table -AutoSize
$failed = ($results | Where-Object { $_.Tulemus -eq "FAIL" }).Count

Write-Host @"

Jargmised sammud (iga rida eraldi aknas, WebAPI peab esimesena jooksma):

  dotnet run --project KooliProjekt.WebAPI/KooliProjekt.WebAPI.csproj --launch-profile http
  dotnet run --project KooliProjekt.WindowsForms/KooliProjekt.WindowsForms.csproj
  dotnet run --project KooliProjekt.WpfApplication/KooliProjekt.WpfApplication.csproj
  dotnet run --project KooliProjekt.BlazorWasm/KooliProjekt.BlazorWasm.csproj --launch-profile http

Tutorialid (26.02):
  dotnet run --project Tutorials/PictureViewer/PictureViewer.csproj
  dotnet run --project Tutorials/MathQuiz/MathQuiz.csproj
  dotnet run --project Tutorials/MatchingGame/MatchingGame.csproj

"@ -ForegroundColor Gray

if ($failed -gt 0) { exit 1 } else { exit 0 }
