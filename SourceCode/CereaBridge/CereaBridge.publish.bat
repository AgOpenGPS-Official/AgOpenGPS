@echo off
setlocal
pushd %~dp0

echo Publishing CereaBridge...
dotnet publish CereaBridge.csproj -c Release -o publish

popd
endlocal
