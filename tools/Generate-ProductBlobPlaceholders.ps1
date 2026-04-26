#Requires -Version 5.1
<#
  Downloads one image per catalog product slug as "{slug}.jpg" for Azure Blob.

  Modes:
    -Mode Pexels      (recommended) uses slug-based search via Pexels API
    -Mode LoremFlickr fallback/dev placeholders without API key
    -Mode Replicate   copy one image to all filenames (fastest)

  Usage:
    $env:PEXELS_API_KEY="your_key"
    powershell -NoProfile -ExecutionPolicy Bypass -File tools/Generate-ProductBlobPlaceholders.ps1 -Mode Pexels
#>
[CmdletBinding()]
param(
    [ValidateSet("Pexels", "LoremFlickr", "Replicate")]
    [string] $Mode = "Pexels",
    [string] $OutDir = "",
    [int] $DelayMs = 1200,
    [string] $SourceUrl = "https://picsum.photos/600/400",
    [string] $PexelsApiKey = ""
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"
$UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"

$root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$productSeed = Join-Path $root "src\services\Database\RetailHub.Database\Tables\Product\Seed\Product.Test.Seed.sql"
if (-not (Test-Path $productSeed)) { throw "Product seed not found: $productSeed" }

if ([string]::IsNullOrWhiteSpace($OutDir)) {
    $OutDir = Join-Path $PSScriptRoot "blob-product-images"
}
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

$text = Get-Content -LiteralPath $productSeed -Raw
$re = "N'([a-z0-9]+(-[a-z0-9]+)+)'"
$slugs = [System.Collections.Generic.HashSet[string]]::new()
foreach ($m in [regex]::Matches($text, $re)) {
    $s = $m.Groups[1].Value
    if ($s -notmatch "4000-8000") { [void]$slugs.Add($s) }
}
if ($slugs.Count -eq 0) { throw "No slugs parsed from Product.Test.Seed.sql" }
$sorted = $slugs | Sort-Object { $_ }

$tagOrSets = @(
    "carp,angling,fish,water"
    "fishing,rod,tackle,reel"
    "carp,fish,lake,river"
    "angling,match,fishing,hook"
    "fishing,boat,net,water"
    "carp,boilie,rig,rod"
    "tackle,shop,bait,fish"
    "fishing,lure,cast,river"
    "carp,pond,reed,fin"
    "fishing,chair,session,bivvy"
)

function Test-IsJpeg {
    param([string] $FilePath)
    $fs = [System.IO.File]::OpenRead($FilePath)
    try {
        $b = New-Object byte[] 2
        [void]$fs.Read($b, 0, 2)
        return ($b[0] -eq 0xFF -and $b[1] -eq 0xD8)
    } finally {
        $fs.Close()
    }
}

function Get-StablePage {
    param([string]$Text)
    $hash = [Math]::Abs($Text.GetHashCode())
    return ($hash % 20) + 1
}

function Save-ImageFromPexels {
    param([string]$Slug, [string]$OutPath, [string]$ApiKey)
    if ([string]::IsNullOrWhiteSpace($ApiKey)) {
        throw "Pexels mode requires -PexelsApiKey or env var PEXELS_API_KEY."
    }

    $query = ($Slug -replace "-", " ") + " carp fishing product"
    $page = Get-StablePage -Text $Slug
    $encoded = [uri]::EscapeDataString($query)
    $searchUrl = "https://api.pexels.com/v1/search?query=$encoded&per_page=1&page=$page&orientation=landscape&size=medium"
    $headers = @{ Authorization = $ApiKey }

    $resp = Invoke-RestMethod -Uri $searchUrl -Headers $headers -UserAgent $UserAgent -TimeoutSec 90
    if (-not $resp.photos -or $resp.photos.Count -eq 0) {
        $fallbackQuery = [uri]::EscapeDataString("carp fishing tackle product")
        $searchUrl = "https://api.pexels.com/v1/search?query=$fallbackQuery&per_page=1&page=$page&orientation=landscape&size=medium"
        $resp = Invoke-RestMethod -Uri $searchUrl -Headers $headers -UserAgent $UserAgent -TimeoutSec 90
    }
    if (-not $resp.photos -or $resp.photos.Count -eq 0) {
        throw "No Pexels photos returned for slug '$Slug'."
    }

    $downloadUrl = $resp.photos[0].src.large2x
    if ([string]::IsNullOrWhiteSpace($downloadUrl)) { $downloadUrl = $resp.photos[0].src.large }
    if ([string]::IsNullOrWhiteSpace($downloadUrl)) { $downloadUrl = $resp.photos[0].src.original }
    Invoke-WebRequest -Uri $downloadUrl -OutFile $OutPath -UseBasicParsing -UserAgent $UserAgent -MaximumRedirection 5 -TimeoutSec 90
}

function Save-ImageFromLoremFlickr {
    param([string]$Slug, [string]$OutPath, [int]$IndexOneBased)
    $tagOr = $tagOrSets[($IndexOneBased - 1) % $tagOrSets.Count]
    $url = "https://loremflickr.com/600/400/{0}?lock={1}" -f $tagOr, $IndexOneBased
    Invoke-WebRequest -Uri $url -OutFile $OutPath -UseBasicParsing -UserAgent $UserAgent -MaximumRedirection 5 -TimeoutSec 90
}

if ($Mode -eq "Replicate") {
    $template = Join-Path $OutDir "._template.jpg"
    if (Test-Path -LiteralPath $SourceUrl) {
        Copy-Item -LiteralPath $SourceUrl -Destination $template -Force
    } else {
        Invoke-WebRequest -Uri $SourceUrl -OutFile $template -UseBasicParsing -UserAgent $UserAgent -MaximumRedirection 5
    }
    $n = 0
    foreach ($slug in $sorted) {
        Copy-Item -LiteralPath $template -Destination (Join-Path $OutDir "$slug.jpg") -Force
        $n++
    }
    Write-Host "Wrote $n files (replicate) to $OutDir" -ForegroundColor Green
    return
}

if ($Mode -eq "Pexels" -and [string]::IsNullOrWhiteSpace($PexelsApiKey)) {
    $PexelsApiKey = $env:PEXELS_API_KEY
}

$n = 0
$idx = 0
$total = $sorted.Count
foreach ($slug in $sorted) {
    $idx++
    $dest = Join-Path $OutDir "$slug.jpg"
    Write-Progress -Activity "$Mode image download" -Status "$idx / $total  $slug" -PercentComplete ([Math]::Min(100, [int](100.0 * $idx / $total)))
    try {
        if ($Mode -eq "Pexels") {
            Save-ImageFromPexels -Slug $slug -OutPath $dest -ApiKey $PexelsApiKey
        } else {
            try {
                Save-ImageFromLoremFlickr -Slug $slug -OutPath $dest -IndexOneBased $idx
            } catch {
                $fallback = "https://picsum.photos/seed/{0}/600/400" -f $slug
                Invoke-WebRequest -Uri $fallback -OutFile $dest -UseBasicParsing -UserAgent $UserAgent -MaximumRedirection 5 -TimeoutSec 90
            }
        }

        if (-not (Test-IsJpeg -FilePath $dest)) {
            $fallback = "https://picsum.photos/seed/{0}/600/400" -f $slug
            Invoke-WebRequest -Uri $fallback -OutFile $dest -UseBasicParsing -UserAgent $UserAgent -MaximumRedirection 5 -TimeoutSec 90
        }
    } catch {
        Write-Error "Failed for ${slug}: $_"
    }
    if ($DelayMs -gt 0) { Start-Sleep -Milliseconds $DelayMs }
    $n++
}
Write-Progress -Activity "$Mode image download" -Completed
Write-Host "Wrote $n files to $OutDir" -ForegroundColor Green
Write-Host "Upload *.jpg to Azure container 'product-images'. Check image licensing before production use."
