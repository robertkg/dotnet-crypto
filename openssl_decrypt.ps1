$ErrorActionPreference = 'Stop'

openssl rsautl -inkey keys/id_rsa -decrypt -pkcs -in random_key.enc -out random_key.tmp
openssl base64 -in random_key.tmp -out random_key

Remove-Item random_key.tmp
$key = Get-Content random_key
Write-Output "random_key: $key"

Write-Output "Decrypting export.zip"
& 'C:\Program Files\7-Zip\7z.exe' x -y -oexport -p"$key" export.zip

Write-Output "`nDecrypted file contents [export/data/fruits.json]:`n"
Get-Content -Raw -Encoding utf8 export/data/fruits.json
