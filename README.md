# dotnet-crypto

## Encrypt

Generate key RSA key pair:

```
ssh-keygen -t rsa -b 4096 -C dotnet-crypto-example -N "test123" -m pem -f keys/id_rsa
```

Convert public key to PEM format:

```
ssh-keygen -f keys/id_rsa.pub -m PEM -e > keys/id_rsa.pub.pem
```

Run the app:

```
dotnet run
```

## Decrypt

```powershell
./openssl_decrypt.ps1
```

