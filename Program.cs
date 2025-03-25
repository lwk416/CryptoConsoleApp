using System.Data;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        DataTable dataTable = new DataTable("Transaction");
        dataTable.Columns.Add("TransactionID", typeof(Guid));
        dataTable.Columns.Add("Amount", typeof(decimal));
        dataTable.Columns.Add("Currency", typeof(string));
        dataTable.Columns.Add("Timestamp", typeof(string));
        dataTable.Rows.Add(Guid.NewGuid(), 100.00, "USD", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        dataTable.Rows.Add(Guid.NewGuid(), 50.00, "MYR", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        dataTable.Rows.Add(Guid.NewGuid(), 10.00, "JPY", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));

        string jsonTable = JsonConvert.SerializeObject(dataTable);
        string base64EncodedData = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonTable));

        Console.WriteLine("Base64 Encoded Data : " + base64EncodedData);

        string sha256Hash = ComputeSha256Hash(base64EncodedData); 

        Console.WriteLine("SHA-256 Hash : " + sha256Hash);   

        string base64EncodedSha256Hash = Convert.ToBase64String(Encoding.UTF8.GetBytes(sha256Hash));

        Console.WriteLine("Base64 Encoded SHA-256 Hash: " + base64EncodedSha256Hash);

        string certificatePath = Path.Combine(AppContext.BaseDirectory, "my-certificate.pfx");
        string digitalSign = SignHash(base64EncodedSha256Hash, certificatePath, "CrytographicSecurity");

        Console.WriteLine("Digital Signature : " + digitalSign);

        var result = new
        {
            EncodedData = base64EncodedData,
            EncodedHash = base64EncodedSha256Hash,
            DigitalSign = digitalSign
        };

        string outputJson = JsonConvert.SerializeObject(result, Formatting.Indented);

        Console.WriteLine("Output JSON : " + outputJson);
    }

    static string ComputeSha256Hash(string encodedData)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(encodedData));
            
            return BitConverter.ToString(bytes).Replace("-", "").ToLower(); 
        }
    }

    static string SignHash(string encodedHash, string certificatePath, string password)
    {
        X509Certificate2 certificate = new X509Certificate2(certificatePath, password);

        if (certificate == null)
        {
            throw new InvalidOperationException("Certificate not found.");
        }

        using (RSA? rsa = certificate.GetRSAPrivateKey())
        {
            if (rsa == null)
            {
                throw new InvalidOperationException("RSA private key not found.");
            }   

            byte[] bytes = Encoding.UTF8.GetBytes(encodedHash);
            byte[] signature = rsa.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signature);
        }
    }
}