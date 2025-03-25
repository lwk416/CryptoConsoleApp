# Secure Data Processing Project

This project processes transaction data by converting it to JSON, applying cryptographic security, and preparing it for secure storage and transmission.

## 🛠️ Features

1. **Data Conversion**: Converts a `DataTable` with transaction details to a JSON string.
2. **Base64 Encoding**: Encodes the JSON data using Base64.
3. **SHA-256 Hashing**: Applies SHA-256 hashing on the encoded data.
4. **Digital Signing**: Signs the hash using an X.509 `.pfx` certificate.
5. **Output**: Outputs a JSON object containing the encoded data, hash, and digital signature.

## 📂 Project Structure
```
|-- SecureDataProject/
    |-- Program.cs
    |-- my-certificate.pfx (Not included for security)
    |-- README.md
    |-- .gitignore
```

## 📊 Data Structure
The DataTable contains the following columns:

- **TransactionID**: Unique identifier (GUID)
- **Amount**: Transaction amount (decimal)
- **Currency**: Transaction currency (string)
- **Timestamp**: Date and time in ISO 8601 format (yyyy-MM-ddTHH:mm:ssZ)

## 🧰 Prerequisites

Ensure you have the following installed:

- **.NET SDK** (>= 6.0)
- **Visual Studio** 
- **Git** (for version control)

## 📥 Setup Instructions

1. Clone the repository:

2. Ensure you have a valid `.pfx` certificate:

You can generate a self-signed certificate using **PowerShell**:

```powershell
New-SelfSignedCertificate -Type Custom -Subject "CN=SecureData" -KeyUsage DigitalSignature -CertStoreLocation "Cert:\CurrentUser\My"
$cert = Get-ChildItem -Path "Cert:\CurrentUser\My" | Where-Object { $_.Subject -eq "CN=SecureData" }
$password = ConvertTo-SecureString -String "yourpassword" -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath ".\my-certificate.pfx" -Password $password
```

Place the `my-certificate.pfx` in the **project root**.

3. Update the `Program.cs` with the correct certificate path and password:

```csharp
string certificatePath = Path.Combine(AppContext.BaseDirectory, "my-certificate.pfx");
string password = "yourpassword";
```

## ▶️ Run the Application

1. Restore dependencies (if any):

2. Build and run the application:
3. 

## 📊 Example Output

```json
{
  "EncodedData": "eyJUcmFuc2FjdGlvbklEIjoiLi4uIiwgIkFtb3VudCI6MTAwLjAsICJDdXJyZW5jeSI6IlVTRCIsICJUaW1lc3RhbXAiOiIyMDI0LTAxLTAxVDAwOjAwOjAwWiJ9",
  "EncodedHash": "X2ZrS0pZR1FhQlNmVGpQVG5uNnZLQT09",
  "DigitalSign": "RW5jcnlwdGVkU2lnbmF0dXJlSGVyZQ=="
}
```

## 🧹 Clean Up

Remove the certificate after testing to keep your system secure:

```powershell
Remove-Item "Cert:\CurrentUser\My\<Thumbprint>"
```

## 📌 Notes

- Modify the **certificate path** and **password** in `Program.cs` as needed.
