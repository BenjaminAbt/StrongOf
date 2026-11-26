param([string]$Configuration = "Release", [switch]$NoBuild)

$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot
$results = Join-Path $root ("TestResults/coverage-" + [Guid]::NewGuid())
$solution = Join-Path $root "BenjaminAbt.StrongOf.slnx"

# Each test module instruments its own copy of the dependencies. Merge line and
# branch hits across modules and framework versions before applying the repository target.
$buildOptions = @()
if ($NoBuild) { $buildOptions += '--no-build' }
dotnet test @buildOptions --solution $solution -c $Configuration --max-parallel-test-modules 1 --results-directory $results --coverlet --coverlet-include '[StrongOf*]*' --coverlet-exclude '[*Tests]*' --coverlet-exclude-by-file '**/*.g.cs' --coverlet-output-format cobertura --report-xunit-trx
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$invariant = [cultureinfo]::InvariantCulture
$packageLines = @{}
$packageBranches = @{}
foreach ($report in (Get-ChildItem $results -Filter 'coverage.cobertura.*.xml')) {
    [xml]$coverage = Get-Content -LiteralPath $report.FullName -Raw
    foreach ($package in @($coverage.coverage.packages.package)) {
        if (-not $package) { continue }
        $pkgName = [string]$package.name
        if (-not $packageLines.ContainsKey($pkgName)) {
            $packageLines[$pkgName] = @{}
            $packageBranches[$pkgName] = @{}
        }

        $linesMap = $packageLines[$pkgName]
        $branchesMap = $packageBranches[$pkgName]

        foreach ($class in @($package.classes.class)) {
            if (-not $class) { continue }
            foreach ($line in @($class.lines.line)) {
                if (-not $line) { continue }
                $key = "$($class.filename):$($line.number)"
                $hits = [long]::Parse([string]$line.hits, $invariant)
                if (-not $linesMap.ContainsKey($key) -or $hits -gt $linesMap[$key]) {
                    $linesMap[$key] = $hits
                }

                if ([string]$line.branch -eq 'True' -and [string]$line.'condition-coverage' -match '\((\d+)/(\d+)\)') {
                    $covered = [int]::Parse($Matches[1], $invariant)
                    $total = [int]::Parse($Matches[2], $invariant)
                    if (-not $branchesMap.ContainsKey($key) -or $covered -gt $branchesMap[$key][0]) {
                        $branchesMap[$key] = @($covered, $total)
                    }
                }
            }
        }
    }
}

$totalLines = 0
$coveredLines = 0
$totalBranches = 0
$coveredBranches = 0
$failed = $false

foreach ($project in (Get-ChildItem (Join-Path $root 'src/*/*.csproj') | Sort-Object BaseName)) {
    $name = $project.BaseName
    if (-not $packageLines.ContainsKey($name) -or $packageLines[$name].Count -eq 0) {
        Write-Host "Package ${name}: missing from the coverage reports"
        $failed = $true
        continue
    }

    $pkgTotalLines = $packageLines[$name].Count
    $pkgCoveredLines = @($packageLines[$name].Values | Where-Object { $_ -gt 0 }).Count
    $pkgLineRate = ($pkgCoveredLines / [double]$pkgTotalLines) * 100.0

    $pkgTotalBranches = 0
    $pkgCoveredBranches = 0
    foreach ($b in $packageBranches[$name].Values) {
        $pkgCoveredBranches += $b[0]
        $pkgTotalBranches += $b[1]
    }
    $pkgBranchRate = if ($pkgTotalBranches -gt 0) { ($pkgCoveredBranches / [double]$pkgTotalBranches) * 100.0 } else { 100.0 }

    $totalLines += $pkgTotalLines
    $coveredLines += $pkgCoveredLines
    $totalBranches += $pkgTotalBranches
    $coveredBranches += $pkgCoveredBranches

    Write-Host ([string]::Format($invariant, 'Package {0}: line {1:F2} % ({2}/{3}), branch {4:F2} % ({5}/{6})', $name, $pkgLineRate, $pkgCoveredLines, $pkgTotalLines, $pkgBranchRate, $pkgCoveredBranches, $pkgTotalBranches))

    if ($pkgCoveredLines -lt $pkgTotalLines -or $pkgCoveredBranches -lt $pkgTotalBranches) {
        $failed = $true
    }
}

if ($totalLines -eq 0) { throw "No executable lines in $results" }

$overallLineRate = ($coveredLines / [double]$totalLines) * 100.0
$overallBranchRate = if ($totalBranches -gt 0) { ($coveredBranches / [double]$totalBranches) * 100.0 } else { 100.0 }
Write-Host ([string]::Format($invariant, 'Line coverage:   {0:F2} % ({1}/{2})', $overallLineRate, $coveredLines, $totalLines))
Write-Host ([string]::Format($invariant, 'Branch coverage: {0:F2} % ({1}/{2})', $overallBranchRate, $coveredBranches, $totalBranches))
Write-Host "Reports in $results"

if ($failed) {
    $missedLines = $totalLines - $coveredLines
    $missedBranches = $totalBranches - $coveredBranches
    throw "Coverage is below 100%: $missedLines missed line(s), $missedBranches missed branch(es)."
}
