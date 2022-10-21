using System.Security.Cryptography;
using System.Diagnostics;
using Ionic.Zip;

var rsa = RSA.Create();
var aes = Aes.Create();

var sampleFileType = Int32.Parse(args[0]);
var sampleFilePath = "";

switch (sampleFileType)
{
    case 1:
        sampleFilePath = "data/sample_100mb";
        break;
    case 2:
        sampleFilePath = "data/sample_1gb";
        break;
    case 3:
        sampleFilePath = "data/sample_10gb";
        break;
    case 4:
        sampleFilePath = "data/sample_20gb";
        break;
    case 5:
        sampleFilePath = "data/sample_50gb";
        break;
    default:
        Console.WriteLine($"Unknown argument: Expected value in range 1..5, got {sampleFileType}");
        throw new IndexOutOfRangeException();
}

Console.WriteLine($"Using sample file {sampleFilePath}");

// Retrieve RSA public key
var file = File.ReadAllText("keys/id_rsa.pub.pem");
rsa.ImportFromPem(file);
Console.WriteLine("RSA public key [keys/id_rsa.pub.pem]:");
Console.WriteLine(file);

// Generate random key for zip encryption
aes.GenerateKey();
var randomKey = aes.Key;
var randomKeyBase64 = Convert.ToBase64String(randomKey);
Console.WriteLine($"randomKey [Base64]:\n\t{randomKeyBase64}");

// Encrypt random key with RSA public key
var randomKeyEncrypted = rsa.Encrypt(randomKey, RSAEncryptionPadding.Pkcs1);
var randomKeyEncryptedBase64 = Convert.ToBase64String(randomKeyEncrypted);
Console.WriteLine($"randomKeyEncrypted [Base64]:\n\t{randomKeyEncryptedBase64}");

// Create encrypted archive
var zip = new ZipFile();
var timer = new Stopwatch();
var zipFileName = "export.zip";

zip.Encryption = EncryptionAlgorithm.WinZipAes256;
zip.UseZip64WhenSaving = Zip64Option.AsNecessary; // >10 GB sample files will cause Ionic.Zip.ZipException

Console.WriteLine($"\nEncrypting {zipFileName} with randomKey");
zip.Password = randomKeyBase64;

Console.WriteLine($"Adding {sampleFilePath}, data/hash.json to archive {zipFileName}");
zip.AddFile(sampleFilePath);
zip.AddFile("data/hash.json");

Console.WriteLine($"Creating archive {zipFileName}...");
timer.Start();
zip.Save(zipFileName);
timer.Stop();
Console.WriteLine($"Completed [Encryption time: {timer.Elapsed.ToString()}]\n");

// Write encrypted random key to file
var randomKeyFilePath = "random_key.enc";
Console.WriteLine($"Writing randomKeyEncrypted to {randomKeyFilePath}");
//File.WriteAllText(randomKeyFilePath, randomKeyEncryptedBase64);
File.WriteAllBytes(randomKeyFilePath, randomKeyEncrypted);

Console.WriteLine("To decrypt using openssl, run the script:");
Console.WriteLine("\t./openssl_decrypt.ps1");
Console.WriteLine("\nFinished dotnet-crypto");

/////////////////////////////////////////////////////////////////////////////////////////////////

// Sample code to decrypt randomKey
// Console.WriteLine("\n\nDecrypting using RSA private key [keys/id_rsa]");
// var privateKeyFile = File.ReadAllText("keys/id_rsa");
// rsa.ImportFromEncryptedPem(privateKeyFile, Encoding.UTF8.GetBytes("test123"));
// var privateKey = File.ReadAllBytes(randomKeyFilePath);
// var randomKeyDecrypted = rsa.Decrypt(privateKey, RSAEncryptionPadding.Pkcs1);
// Console.WriteLine($"randomKeyDecrypted:\n\t{Convert.ToBase64String(randomKeyDecrypted)}");
