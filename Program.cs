using System.IO;
using System.Security.Cryptography;
// using DotNetZip;

var rsa = RSA.Create();
var aes = Aes.Create();

// Retrieve RSA public key
var file = File.ReadAllText("keys/id_rsa.pub.pem");
rsa.ImportFromPem(file); // PEM PKCS#1 format
Console.WriteLine("RSA public key [keys/id_rsa.pub.pem]:");
Console.WriteLine(file);

// Generate random key for zip encryption
aes.GenerateKey();
var randomKey = aes.Key;
var randomKeyBase64 = Convert.ToBase64String(aes.Key);
Console.WriteLine($"randomKey:\n\t{randomKeyBase64}");

// Encrypt random key with RSA public key
var randomKeyEncrypted = rsa.Encrypt(randomKey, RSAEncryptionPadding.Pkcs1);
var randomKeyEncryptedBase64 = Convert.ToBase64String(randomKeyEncrypted);
Console.WriteLine($"randomKeyEncrypted:\n\t{randomKeyEncryptedBase64}");

// Create encrypted zip
Console.WriteLine("\nEncrypting export.zip with randomKey");
// var zip = new ZipFile();
// zip.Password = randomKeyBase64
// zip.Save("export.zip")

// Write encrypted random key to file
var randomKeyFilePath = "random_key.enc";
Console.WriteLine($"Writing randomKeyEncrypted to {randomKeyFilePath}");
File.WriteAllText(randomKeyFilePath, randomKeyEncryptedBase64);
