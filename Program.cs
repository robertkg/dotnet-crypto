using System.Security.Cryptography;
using System.Text;
using Ionic.Zip;

var rsa = RSA.Create();
var aes = Aes.Create();

// Retrieve RSA public key
var file = File.ReadAllText("keys/id_rsa.pub.pem");
rsa.ImportFromPem(file); // PEM PKCS#1 format
Console.WriteLine("RSA public key [keys/id_rsa.pub.pem]:");
Console.WriteLine(file);


// Generate random key for zip encryption
//var randomKeyPlaintext = "password1234";
//var randomKey = Encoding.UTF8.GetBytes(randomKeyPlaintext);
aes.GenerateKey();
var randomKey = aes.Key;
var randomKeyBase64 = Convert.ToBase64String(randomKey);
Console.WriteLine($"randomKey:\n\t{randomKeyBase64}");

// Encrypt random key with RSA public key
var randomKeyEncrypted = rsa.Encrypt(randomKey, RSAEncryptionPadding.Pkcs1);
var randomKeyEncryptedBase64 = Convert.ToBase64String(randomKeyEncrypted);
Console.WriteLine($"randomKeyEncrypted:\n\t{randomKeyEncryptedBase64}");

// Create encrypted archive
Console.WriteLine("\nEncrypting export.zip with randomKey");
var zip = new ZipFile();
zip.Password = randomKeyBase64;
zip.AddFile("data/fruits.json");
zip.Save("export.zip");

// Write encrypted random key to file
var randomKeyFilePath = "random_key.enc";
Console.WriteLine($"Writing randomKeyEncrypted to {randomKeyFilePath}");
//File.WriteAllText(randomKeyFilePath, randomKeyEncryptedBase64);
File.WriteAllBytes(randomKeyFilePath, randomKeyEncrypted);

Console.WriteLine("\nTo decrypt using openssl, run the script:");
Console.WriteLine("\t./openssl_decrypt.ps1");

// Sample code to decrypt RSA
// Console.WriteLine("\n\nDecrypting using RSA private key [keys/id_rsa]");
// var privateKeyFile = File.ReadAllText("keys/id_rsa");
// rsa.ImportFromEncryptedPem(privateKeyFile, Encoding.UTF8.GetBytes("test123"));

// var privateKey = File.ReadAllBytes(randomKeyFilePath);
// var randomKeyDecrypted = rsa.Decrypt(privateKey, RSAEncryptionPadding.Pkcs1);
// Console.WriteLine($"randomKeyDecrypted:\n\t{Convert.ToBase64String(randomKeyDecrypted)}");
