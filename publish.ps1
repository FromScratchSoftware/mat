$key = gc .\.env

cd src
$csproj = gc .\Scratch.Mathematics.Mat.csproj
$versionText = $csproj | % {
    if ($_.Contains("PackageVersion"))
    {
        $_
    }
}

$version = ""
$flag = 0
for ($i = 0; $i -lt $versionText.Length; $i++)
{
    $char = $versionText[$i]

    if ($flag -eq 1)
    {
        if ($char -eq "<")
        {
            break
        }

        $version += $char
    }

    if ($char -eq ">")
    {
        $flag = 1
    }
}

dotnet pack -c Release
$file = ".\bin\Release\Scratch.Mathematics.Mat." + $version + ".nupkg"
cp $file Scratch.Mathematics.Mat.nupkg

dotnet nuget push Scratch.Mathematics.Mat.nupkg --api-key $key --source https://api.nuget.org/v3/index.json
rm .\Scratch.Mathematics.Mat.nupkg
cd ..