#### Publish for linux-arm64

`dotnet publish -c Release -o ./publish -r linux-arm64 -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained=true`
