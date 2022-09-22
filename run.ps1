Write-Output "`n--- Genering keys"
ssh-keygen -t rsa -b 4096 -C dotnet-crypto-example -N "test123" -m pem -f keys/id_rsa
ssh-keygen -f keys/id_rsa.pub -m PEM -e > keys/id_rsa.pub.pem

Write-Output "`n--- Running dotnet app"
dotnet run

"`n--- Running openssl_decrypt.ps1"
& $PSScriptRoot/openssl_decrypt.ps1