$sslPassFilePath = ".\.sslpass"

# Read the password from the .sslpass file
$sslPassword = Get-Content -Path $sslPassFilePath -Raw

dotnet dev-certs https -ep .\certs\aspnetapp.pfx -p $sslPassword

dotnet dev-certs https --trust