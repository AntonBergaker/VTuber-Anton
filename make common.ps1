$Dir = "./VTuberAnton.Common"
$Target = "$Dir/bin/Release"
$TargetFile = "$Target/VTuberAnton.Common.dll"
dotnet build "$Dir/VTuberAnton.Common.csproj" -o "$Target/" -c Release

Copy-Item -Path $TargetFile -Destination "./VTuber Anton Server/Assets/Plugins"
Copy-Item -Path $TargetFile -Destination "./VTuber Anton Client/Assets/Plugins"