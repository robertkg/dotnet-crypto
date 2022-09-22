# dotnet-crypto

Example project for symmetric and asymmetric encryption in .NET 6.

- Using `AesManaged.GenerateKey` from `System.Security.Cryptography` namepsace for symmetric encryption
- Using RSA 4096-bit PKCS#1 w/ v1.5 padding keys in PEM format for asymmetric encryption

## Encrypt

Generate key RSA key pair:

```
ssh-keygen -t rsa -b 4096 -a 100 -C dotnet-crypto -N "test123" -o -m pem -f keys/id_rsa
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

