﻿$dirInfo= New-Object -Typename System.IO.DirectoryInfo -ArgumentList ([System.IO.Directory]::GetCurrentDirectory())
$projectName= $dirInfo.Name;
Set-Location $dirInfo.Parent.FullName
$key=$env:nugetKey
$commit=($(git log --format="%H" -n 1) | Out-String).Trim()
$branch=($(git rev-parse --abbrev-ref HEAD) | Out-String).Trim()
$buildDay=[DateTime]::Now.ToString("yyyyMMdd")
$buildIndex="00"
$p="branch=$($branch);commit=$($commit);buildDay=$($buildDay);buildIndex=$($buildIndex)".Trim()

function RunCommand
{
    $numOfArgs = $args.Length
    for ($i=0; $i -lt $numOfArgs; $i++)
    {
        iex $args[$i]
        if($LASTEXITCODE -eq 0) {
            Write-Host "$($args[$i]) success"
        }
        else{
            Write-Host "$($args[$i]) failed"
            return 0
        }
    }
    return 1
}

function NugetPack
{
    $numOfArgs = $args.Length
    for ($i=0; $i -lt $numOfArgs; $i++)
    {
        Write-Host "NugetPack $($args[$i])"

        $result = RunCommand "Remove-Item -Recurse -Force .\$($args[$i])\bin\Release\**" `
            "dotnet build $($args[$i])\$($args[$i]).csproj -c Release" `
            "nuget pack $($args[$i])\$($args[$i]).nuspec -OutputDirectory .\$($args[$i])\bin\Release -p 'id=$($args[$i]);$($p)'"

        if($result) {
            Write-Host "$($args[$i]) success"
        }
        else{
            Write-Host "$($args[$i]) failed"
            return 0
        }
    }
    return 1 
}

function NugetPush
{
    $numOfArgs = $args.Length
    for ($i=0; $i -lt $numOfArgs; $i++)
    {
        Write-Host "NugetPush $($args[$i])"

        iex "dotnet nuget push $($args[$i])\bin\Release\*.nupkg --api-key $($key) --source https://api.nuget.org/v3/index.json"
    }
}

$result = NugetPack $projectName
if($result)
{
    if([string]::IsNullOrEmpty($key))
    {
        Write-Host "Build & pack success"
    }
    else
    {
        Write-Host "enter to push nuget"
        pause
        Write-Host "enter to confirm"
        pause

        NugetPush $projectName
    }
}
else
{
    echo "Build & pack error"
}
pause