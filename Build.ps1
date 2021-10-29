$current = Resolve-Path($PSScriptRoot)

$tools = Join-Path $current tools
$vswhere = Join-Path $tools vswhere.exe

if (!(Test-Path(${vswhere})))
{
    Write-Host "[Error] ${vswhere} is missing" -ForegroundColor Red
    exit -1
}

$msbuild = & "${vswhere}" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | select-object -first 1
if (!($msbuild))
{
    Write-Host "[Error] msbuild.exe is missing" -ForegroundColor Red
    exit -2
}
Write-Host "[Info] ${vswhere} is found" -ForegroundColor Green

$solution = Join-Path $current source | `
            Join-Path -ChildPath RedArmory.sln
if (!(Test-Path(${solution})))
{
    Write-Host "[Error] ${solution} is missing" -ForegroundColor Red
    exit -3
}
Write-Host "[Info] ${solution} is found" -ForegroundColor Green

$licensesDir = Join-Path $current licenses
if (!(Test-Path(${licensesDir})))
{
    Write-Host "[Error] ${licensesDir} is missing" -ForegroundColor Red
    exit -4
}
Write-Host "[Info] ${licensesDir} is found" -ForegroundColor Green

$configuration = "Release"
Write-Host "[Info] clean ${solution}" -ForegroundColor Green
& "${msbuild}" "${solution}" /t:clean /p:Configuration=$configuration /p:Platform="Any CPU" -verbosity:minimal
Write-Host "[Info] build ${solution}" -ForegroundColor Green
& "${msbuild}" "${solution}" /t:build /p:Configuration=$configuration /p:Platform="Any CPU" -verbosity:minimal

$buildDir = Join-Path $current source | `
            Join-Path -ChildPath RedArmory | `
            Join-Path -ChildPath bin | `
            Join-Path -ChildPath $configuration
if (!($buildDir))
{
    Write-Host "[Error] ${buildDir} is missing" -ForegroundColor Red
    exit -2
}

$artifacts = Join-Path $current artifacts
New-Item $artifacts -Type Directory -Force | Out-Null

Write-Host "[Info] copy build files to ${artifacts}" -ForegroundColor Green
Remove-Item "${artifacts}/*" -Force -Recurse | Out-Null
Copy-Item "${buildDir}/*" "${artifacts}" -Force -Recurse

# remove files
$directories = @(
    "de",
    "es",
    "fr",
    "it",
    "zh-CN",
    "logs"
)
$files = @(
    "*.xml",
    "*.pdb",
    "*.json"
)

Write-Host "[Info] remove directories from ${artifacts}" -ForegroundColor Green
$directories | Foreach-Object {
    $target = Join-Path "${artifacts}" $_
    if (Test-Path("${target}"))
    {
        Remove-Item "${target}" -Force -Recurse | Out-Null
    }
}

Write-Host "[Info] remove files from ${artifacts}" -ForegroundColor Green
$files | Foreach-Object {
    $target = Join-Path "${artifacts}" $_
    if (Test-Path("${target}"))
    {
        Remove-Item "${target}" -Force | Out-Null
    }
}

Write-Host "[Info] copy license files to ${artifacts}" -ForegroundColor Green
Copy-Item "${licensesDir}" "${artifacts}" -Force -Recurse

Write-Host "[Info] compress ${artifacts} to zip file" -ForegroundColor Green
Compress-Archive -Path "${artifacts}/*" -DestinationPath artifacts.zip

Set-Location $currrent
