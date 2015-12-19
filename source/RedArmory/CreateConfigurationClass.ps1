$cd = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$target = "BitNamiRedmineStackConfiguration.xsd"
$output = "Models.Configurations\"

$proc = New-Object System.Diagnostics.Process
$proc.StartInfo.WorkingDirectory = $cd
$proc.StartInfo.UseShellExecute = $false # Necessary to capture stderr/stdout.
$proc.StartInfo.RedirectStandardOutput = $true
$proc.StartInfo.RedirectStandardError = $true
$proc.StartInfo.FileName = "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\xsd.exe"
$proc.StartInfo.Arguments = "$target /o:$output /l:cs /c /n:RedArmory.Models.Configurations"

$proc.Start() | Out-Null
$output = $proc.StandardOutput.ReadToEnd()
$outputErr = $proc.StandardError.ReadToEnd()
$proc.WaitForExit()