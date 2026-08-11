dotnet publish .\src\NotesProxy.Cli\ -c Release -r win-x64 -o bin

$SourceExe   = (Resolve-Path ".\bin/notesproxy.exe").Path
$TargetDir   = "$env:LocalAppData\Programs\NotesProxy"
$TargetExe   = Join-Path $TargetDir (Get-Item $SourceExe).Name
$TargetAlias = Join-Path $TargetDir np.exe

# 1. Create the application directory if it doesn't exist
if (!(Test-Path $TargetDir)) {
    New-Item -ItemType Directory -Path $TargetDir -Force | Out-Null
}

# 2. Move the executable to the user programs folder
Move-Item -Path $SourceExe -Destination $TargetDir -Force
Copy-Item -Path $TargetExe -Destination $TargetAlias -Force

# 3. Add it to the path if it's not there already
$UserPath = [Environment]::GetEnvironmentVariable("Path", [EnvironmentVariableTarget]::User)
if ($UserPath -split ';' -notcontains $TargetDir) {
    [Environment]::SetEnvironmentVariable("Path", "$UserPath;$TargetDir", [EnvironmentVariableTarget]::User)
    $env:Path += ";$TargetDir" # Update current session
    Write-Host "Successfully added '$TargetDir' to User PATH." -ForegroundColor Green
} else {
    Write-Host "Folder '$TargetDir' is already in User PATH." -ForegroundColor Yellow
}