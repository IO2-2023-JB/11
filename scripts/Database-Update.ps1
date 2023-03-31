Push-Location -Path $PSScriptRoot\..\backend\YouTubeV2.Application

dotnet ef database update --startup-project ..\YouTubeV2
dotnet ef database update --connection "Server=localhost;User Id=sa;Password=4C0b138f;Database=YouTubeV2Test;TrustServerCertificate=True" --startup-project ..\YouTubeV2

Pop-Location