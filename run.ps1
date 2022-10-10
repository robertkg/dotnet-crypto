[CmdletBinding()]
param (
    [Parameter(Mandatory, HelpMessage = "Sample file to test with. Possible values are:`n1 = 100MB`n2 = 1GB`n3 = 10 GB`n4 = 20 GB`n5 = 50 GB")]
    [ValidateRange(1, 5)]
    $SampleFileType
)

$ErrorActionPreference = 'Stop'
$dataDir = "$PSScriptRoot\data"

Remove-Item $dataDir\sample*
Remove-Item $dataDir\hash.json -ErrorAction SilentlyContinue
Remove-Item $PSScriptRoot\export -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item $PSScripRoot\random_key* -ErrorAction SilentlyContinue
Remove-Item $PSScripRoot\export.zip -ErrorAction SilentlyContinue

# 1 MB   = 1048576 bytes
# 100 MB = 104857600 bytes
# 1 GB   = 1073741824 bytes
# 10 GB  = 10737418240 bytes
# 100 GB = 107374182400 bytes
# 1 TB   = 1099511627776 bytes
# 10 TB  = 10995116277760 bytes

switch ($SampleFileType) {
    1 { 
        $sampleFileLength = 104857600
        $sampleFilePath = "$dataDir\sample_100mb"
    }
    2 {
        $sampleFileLength = 1073741824
        $sampleFilePath = "$dataDir\sample_1gb"
    }
    3 {
        $sampleFileLength = 10737418240
        $sampleFilePath = "$dataDir\sample_10gb"
    }
    4 {
        $sampleFileLength = 21474836480
        $sampleFilePath = "$dataDir\sample_20gb"
    }
    5 {
        $sampleFileLength = 53687091200
        $sampleFilePath = "$dataDir\sample_50gb"
    }
    Default {
        Write-Error "Unknown SampleFileType $SampleFileType"
    }
}

Write-Output 'Creating sample file'
fsutil file createnew $sampleFilePath $sampleFileLength
Write-Output 'Computing file hash [SHA1]'
$sampleFileHash = Get-FileHash -Algorithm SHA1 -Path $sampleFilePath | ConvertTo-Json
Write-Output $sampleFileHash
$sampleFileHash > "$dataDir\hash.json"

Write-Output "`nRunning dotnet-crypto"
dotnet run $SampleFileType

Write-Output "`nDecrypting random_key.enc with RSA private key [keys/id_rsa]"
openssl rsautl -inkey keys/id_rsa -decrypt -pkcs -in random_key.enc -out random_key.tmp -passin pass:test123
openssl base64 -in random_key.tmp -out random_key
Remove-Item random_key.tmp
$key = Get-Content random_key
Write-Output "random_key [Base64]:`n`t$key"

$zipFileName = 'export.zip'
Write-Output "Decrypting $zipFileName with random_key..."

$timer = [System.Diagnostics.Stopwatch]::StartNew()
& 'C:\Program Files\7-Zip\7z.exe' x -y -oexport -p"$key" export.zip 1>$null
$timer.Stop()

Write-Output "7z decryption completed with exit code $LASTEXITCODE [Decryption time: $($timer.Elapsed.ToString())]"
Get-Content -Raw "$PSScriptRoot\export\data\hash.json"
