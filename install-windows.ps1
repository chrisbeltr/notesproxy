dotnet publish .\src\NotesProxy.Cli\ -c Release -r win-x64 -o bin

$SourceExe   = (Resolve-Path ".\bin/notesproxy.exe").Path
$TargetDir   = "$env:LocalAppData\Programs\NotesProxy"
$TargetExe   = Join-Path $TargetDir (Get-Item $SourceExe).Name
$TargetAlias = Join-Path $TargetDir np.exe

echo $SourceExe

# 1. Create the application directory if it doesn't exist
if (!(Test-Path $TargetDir)) {
    New-Item -ItemType Directory -Path $TargetDir -Force | Out-Null
}

# 2. Move the executable to the user programs folder
Move-Item -Path $SourceExe -Destination $TargetDir -Force
Copy-Item -Path $TargetExe -Destination $TargetAlias -Force

# 3. Add it to the path if it's not there already
$currentPath = [Environment]::GetEnvironmentVariable("Path", "User")
[Environment]::SetEnvironmentVariable("Path", "$currentPath;$TargetDir", "User")