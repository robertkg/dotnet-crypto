# dotnet-crypto

Example project for symmetric and asymmetric encryption in .NET 6.

- Using `AesManaged.GenerateKey` from `System.Security.Cryptography` namepsace for symmetric encryption
- Using RSA 4096-bit PKCS#1 w/ v1.5 padding keys in PEM format for asymmetric encryption

## Generate RSA key pair

```
ssh-keygen -t rsa -b 4096 -a 100 -C dotnet-crypto -N "test123" -o -m pem -f keys/id_rsa
```

Convert public key to PEM format:

```
ssh-keygen -f keys/id_rsa.pub -m PEM -e > keys/id_rsa.pub.pem
```

## Run the app

```powershell
./run.ps1 -SampleFileType <1,2,3,4,5>
```

The script does the following:

- Create sample files and compute file hash
- Run the `dotnet-crypto` app, which does the following:
  - Generate symmetric random key and encrypts it using RSA public key
  - Create archive `export.zip` and encrypts it using symmetric random key
- Decrypt symmetric key `random_key.enc` with RSA private key using openssl
- Decrypt `export.zip` with symmetric key `random_key` and check computed file hash
